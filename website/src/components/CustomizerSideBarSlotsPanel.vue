<template>
  <div>
    <div class="icon-div-top">
      <i class="material-icons md-12 md-blue btn">help</i>
      <span class="tooltiptext">In this step, you can add divisions to the closet's structure and customize their sizes.</span>
    </div>
    <div class="slotsSelections">
      <label >
        <input type="radio" id="recommendedSlots" value="recommendedSlots" v-model="picked"  @change="deactivateCanvasControls()"> Recommended Number Slots
      </label>
      <label >
        <input type="radio" id="customizedSlots" value="customizedSlots" v-model="picked"  @change="activateCanvasControls()" > Customized Number Slots
      </label>
    </div>
    <div v-if="displaySliders" class="slidersSection">
      <i class="btn btn-primary material-icons" @click="removeLine(index)">remove</i>
      <i class="btn btn-primary material-icons" @click="addLine()">add</i>
      <div class="scroll">
        <div class="scrollable-div" style="height: 215px; width: 270px;">
          <div v-for="(line, index) in lines.slice(0, maxNumberSlots)" v-bind:key="index">
            <vue-slider
              class="slidersEspecification"
              :min="minSizeSlot"
              :max="maxSizeSlot"
              v-model="sliderValues[index]"
              @drag-end="putWidthSlot(index)"
            ></vue-slider>
            <!-- 
              @callback="updateWidthSlot(index)" -->
          </div>
        </div>
      </div>
    </div>
    <div class="center-controls">
      <i class="btn btn-primary material-icons" @click="previousPanel()" >arrow_back</i>
      <i class="btn btn-primary material-icons" @click="nextPanel()" >arrow_forward</i>
    </div>
  </div>
</template>
<script>
import vueSlider from "vue-slider-component";
import store from "./../store";
import UnitRequests from "./../services/mycm_api/requests/units.js";
import CustomizedProductRequests from "./../services/mycm_api/requests/customizedproducts.js";
import { ADD_SLOT_DIMENSIONS,SET_ID_SLOT, DEACTIVATE_CAN_MOVE_CLOSET, ACTIVATE_CAN_MOVE_SLOTS, DEACTIVATE_CAN_MOVE_SLOTS } from "./../store/mutation-types.js";
export default {
  name: "CustomizerSideBarSlotsPanel",
  data() {
    return {
      picked: "recommendedSlots",
      numberSlots: 0,
      sliderValues: [],
      lines: [],
      blockRemoval: true,
      slotsToPost: [],
      slotsToDelete: [],
      listRecommendedSlots: [],
      drawCustomizedSlots: [],
      custUpdates: [],
      reasonW: 404.5 / store.getters.customizedProductDimensions.width
    };
  },
  components: {
    vueSlider
  },
  computed: {
    maxSizeSlot(){
      return store.getters.maxSlotWidth
    },
    minSizeSlot(){
       return store.getters.minSlotWidth
    },
    minNumberSlots(){
      var number = parseInt(store.state.customizedProduct.customizedDimensions.width / this.maxSizeSlot);
      return number;
    },
    maxNumberSlots(){
      var number = parseInt(store.state.customizedProduct.customizedDimensions.width/ this.minSizeSlot) - 1;
      return number;
    },
    displaySliders() {
      return this.picked === "customizedSlots";
    }
  },
  methods: {
    addLine() {
      let checkEmptyLines = this.lines.filter(line => line.number === null);
      if (checkEmptyLines.length >= 1 && this.lines.length > 0){
         return;
      }
      if(this.lines.length >= this.maxNumberSlots ){
        this.$toast.open("You have already reached the maximum number of slots");
      }else{
        this.drawOneSlot();
         this.lines.push({
        slider: null
        }) 
       } 
    },
    removeLine(lineId) {
      if (!this.blockRemoval){
         this.lines.splice(lineId, 1); 
         this.removeOneSlot();
      }
    },
    nextPanel(){
      if(this.minNumberSlots <= 0){
        this.$emit("advance");
      }else{
      CustomizedProductRequests.getCustomizedProduct(store.state.customizedProduct.id)
      .then((response) => {
              if(response.data.slots.length > 1){
                this.$emit("advance");
              }
            })
             .catch((error_message) => {
          });
      }
    },
    previousPanel(){
      this.$dialog.confirm({
          title: 'Important Information',
          hasIcon: true,
          type: 'is-info',
          icon: 'fas fa-exclamation-circle size:5px',
          iconPack: 'fa',
          message: 'Are you sure you want to return? All progress made in this step will be lost.',
          onConfirm: () => {
            this.deleteSlots();
            this.$emit("back");
          }
        })
    },
    deleteSlots(){
      store.dispatch(ADD_SLOT_DIMENSIONS)
      let slotsToDelete = [];
      let custProducSlots = [];
      let size = -1
      CustomizedProductRequests.getCustomizedProducts()
        .then((response) => {
          custProducSlots = response.data;
          size = custProducSlots[custProducSlots.length - 1].id;
          CustomizedProductRequests.getCustomizedProduct(size)
          .then((product) => {
            custProducSlots = product.data.slots;
            for(let i = 0; i< custProducSlots.length-1; i++){
              slotsToDelete.unshift(custProducSlots[i].id);
            }
              this.deleteSlot(slotsToDelete)
              .then(() => {})
              .catch((error_message) => {
              });
          })
          .catch((error_message) => {
          });
        })
        .catch((error_message) => {
        });
    },
    deleteSlot(slotsToDelete){
      return new Promise((accept, reject) => {
        let slotToDelete = slotsToDelete.pop();
        CustomizedProductRequests.deleteCustomizedProductSlot(store.state.customizedProduct.id, slotToDelete)
        .then(() => {
          if(slotsToDelete.length > 0 ){
            return this.deleteSlot(slotsToDelete)
            .then(()=>{
              accept()})
            .catch((error_message) => {
             reject(error_message)});
          } else {
             accept();
          }
        })
        .catch((error_message) => {
          reject(error_message.response.data.message);
        });
      })
    },
    activateCanvasControls(){
      this.drawMinSlots();
      store.dispatch(ACTIVATE_CAN_MOVE_SLOTS);
    },
     deactivateCanvasControls(){
      this.drawRecommendedSlots();
      store.dispatch(DEACTIVATE_CAN_MOVE_SLOTS)
    },
    drawRecommendedSlots(){
      this.listRecommendedSlots = [];
      var unitCloset = store.state.customizedProduct.customizedDimensions.unit;
        CustomizedProductRequests.postCustomizedProductRecommendedSlots(store.state.customizedProduct.id)
        .then(response => {
          this.listRecommendedSlots = response.data.slots;
          for (let i = 0; i < this.listRecommendedSlots.length; i++) {
             store.dispatch(ADD_SLOT_DIMENSIONS, {
              idSlot: this.listRecommendedSlots[i].id,
              width: this.listRecommendedSlots[i].dimensions.width * this.reasonW,
              height: this.listRecommendedSlots[i].dimensions.height,
              depth: this.listRecommendedSlots[i].dimensions.depth,
              unit: this.listRecommendedSlots[i].dimensions.unit
            })
          }
        })
        .catch((error_message) => {
          this.$toast.open("There was an error get recommended slots. Please try again!");
        });
    },
    drawMinSlots(){
      store.dispatch(ADD_SLOT_DIMENSIONS)
      this.slotsToPost = [];
      this.sliderValues = [];
      this.lines = [];
       
      CustomizedProductRequests.postCustomizedProductMinimumSlots(store.state.customizedProduct.id)
        .then(response => {
          this.slotsToPost =  response.data.slots;
          for (let i = 0; i < this.slotsToPost.length; i++) {
            store.dispatch(ADD_SLOT_DIMENSIONS, {
              idSlot: this.slotsToPost[i].id,
              width: this.slotsToPost[i].dimensions.width * this.reasonW,
              height: this.slotsToPost[i].dimensions.height,
              depth: this.slotsToPost[i].dimensions.depth,
              unit: this.slotsToPost[i].dimensions.unit
            });
            if(i < this.slotsToPost.length - 1){
                  this.lines.push({
                  slider: null
                  }) 
                  this.sliderValues[i] = this.slotsToPost[i].dimensions.width
            }
          }
        })
        .catch((error_message) => {
          this.$toast.open("There was an error get minimum slots. Please try again!");
        });
    },
    drawOneSlot(){
      var customizedProductDimensions = store.getters.customizedProductDimensions;
      CustomizedProductRequests.postCustomizedProductSlot(store.state.customizedProduct.id,
        {
          height: customizedProductDimensions.height,
          depth:  customizedProductDimensions.depth,
          width: store.getters.minSlotWidth,
          unit:  customizedProductDimensions.unit,
        }).then(response => {
          this.lines = [];
          this.sliderValues = [];
          this.drawCustomizedSlots = [];
          this.drawCustomizedSlots.push(...response.data.slots);
          store.dispatch(ADD_SLOT_DIMENSIONS);
            for(let i=0; i<this.drawCustomizedSlots.length; i++){
              store.dispatch(ADD_SLOT_DIMENSIONS, {
                idSlot: this.drawCustomizedSlots[i].id,
                width: this.drawCustomizedSlots[i].dimensions.width * this.reasonW,
                height: this.drawCustomizedSlots[i].dimensions.height,
                depth: this.drawCustomizedSlots[i].dimensions.depth,
                unit: this.drawCustomizedSlots[i].dimensions.unit,
              })
              if(i < this.drawCustomizedSlots.length -1){
                this.lines.push({
                  slider: null
                })
                this.sliderValues[i] = this.drawCustomizedSlots[i].dimensions.width.toFixed(0);
              }
            }
        })
        .catch((error_message) => {
          this.$toast.open("Unable to draw more slots");
        });
    },
    removeOneSlot(){
      CustomizedProductRequests.deleteCustomizedProductSlot(store.state.customizedProduct.id,this.drawCustomizedSlots[this.drawCustomizedSlots.length-1].id)
      .then(response => {
        this.drawCustomizedSlots.pop();
        store.dispatch(ADD_SLOT_DIMENSIONS);
        this.lines = [];
        this.sliderValues = [];
        for(let i=0; i<this.drawCustomizedSlots.length; i++){
          store.dispatch(ADD_SLOT_DIMENSIONS, {
                  idSlot: this.drawCustomizedSlots[i].id,
                  width: this.drawCustomizedSlots[i].dimensions.width * this.reasonW,
                  height: this.drawCustomizedSlots[i].dimensions.height,
                  depth: this.drawCustomizedSlots[i].dimensions.depth,
                  unit: this.drawCustomizedSlots[i].dimensions.unit,
          });
          if(i < this.drawCustomizedSlots.length -1){
              this.lines.push({
                slider: null
              })
              this.sliderValues[i] = this.drawCustomizedSlots[i].dimensions.width.toFixed(0);
          }
        }
        })
        .catch((error_message) => {this.$toast.open("Unable to draw more slots");});
       
    },
    updateWidthSlot(index){
      var depthCloset = 0;
      var heightCloset = 0;
      var unitCloset = store.state.customizedProduct.customizedDimensions.unit;
      store.dispatch(ADD_SLOT_DIMENSIONS, {
                  idSlot: index,
                  width: this.sliderValues[index] * this.reasonW,
                  height: heightCloset,
                  depth: depthCloset,
                  unit: unitCloset
      });        
    },
    putWidthSlot(index){
      var customizedProductDimensions = store.getters.customizedProductDimensions;
      var slot_to_move = store.state.customizedProduct.slots[index].idSlot;
          CustomizedProductRequests.putCustomizedProductSlot(store.state.customizedProduct.id,slot_to_move, 
          {
            dimensions: {
              height: customizedProductDimensions.height,
              width: this.sliderValues[index],
              depth: customizedProductDimensions.depth,
              unit: customizedProductDimensions.unit,
            }
          })
          .then(response => {
                this.lines = [];
                this.sliderValues = [];
                this.drawCustomizedSlots = [];
                this.drawCustomizedSlots.push(...response.data.slots);
                store.dispatch(ADD_SLOT_DIMENSIONS);
                for(let i=0; i<this.drawCustomizedSlots.length; i++){
                    store.dispatch(ADD_SLOT_DIMENSIONS, {
                      idSlot: this.drawCustomizedSlots[i].id,
                      width: this.drawCustomizedSlots[i].dimensions.width * this.reasonW,
                      height: this.drawCustomizedSlots[i].dimensions.height,
                      depth: this.drawCustomizedSlots[i].dimensions.depth,
                      unit: this.drawCustomizedSlots[i].dimensions.unit,
                    })
                    if(i < this.drawCustomizedSlots.length -1){
                      this.lines.push({
                        slider: null
                      })
                      this.sliderValues[i] = this.drawCustomizedSlots[i].dimensions.width.toFixed(0);
                    }
                  }
              })
            .catch((error_message) => {
              this.lines = [];
              this.sliderValues = [];
                for(let k=0; k<this.drawCustomizedSlots.length; k++){
                    if(k < this.drawCustomizedSlots.length -1){
                      this.lines.push({
                        slider: null
                      })
                      this.sliderValues[k] = this.drawCustomizedSlots[k].dimensions.width.toFixed(0);
                    }
                } 
                 this.$toast.open("The pretended slot width exceeds the maximum width value");
            });
    },
  },
  watch: {
  lines() {
      this.blockRemoval = this.lines.length <= this.minNumberSlots;
    }
  },
  created() {
    if(this.minNumberSlots <= 0){
        this.nextPanel();
    }else{
      this.drawRecommendedSlots()         
      store.dispatch(DEACTIVATE_CAN_MOVE_CLOSET);
    }
  },
}
</script>
<style>
.slotsSelections {
  float: left;
  padding: 7px 20px;
}
.slidersSection {
  margin-bottom: 13%;
  width: 7px 30px;
  margin-left: 5%;
  margin-right: 5%;
}
.slidersEspecification {
  width: 7px 30px;
  margin-left: 5%;
  margin-right: 5%;
  margin-top: 15%
}
.scroll {
  margin-top: 5%
}
.component {
  margin-bottom: 31%;
}
.icon-div-top .tooltiptext {
  visibility: hidden;
  width: 100px;
  background-color: #797979;
  color: #fff;
  border-radius: 6px;
  font-size: 12px;
  padding: 10%;
  position: absolute;
  top: 25px;
  left: 0px;
  right: 0px;
}
.icon-div-top:hover .tooltiptext {
  visibility: visible;
}
.icon-div-top {
  top: 15px;
  left: 15px;
  margin-left: 90px;
  position: absolute;
}
.allignButtons {
  margin-left: 150px ;
}
</style>