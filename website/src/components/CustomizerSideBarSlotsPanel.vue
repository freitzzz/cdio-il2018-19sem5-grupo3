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
              @callback="updateWidthSlot(index)"
            ></vue-slider>
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
      freeSpace: "",
      createNewSlider: false,
      valueConverted: "",
      blockRemoval: true,
      slotsToPost: [],
      slotsToDelete: [],
      listRecommendedSlots: [],
      listMinSlots: [],
      drawCustomizedSlots: [],
      slotsID: [],
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
    convert(from, to, value) {
      UnitRequests.convertValue(from, to, value)
        .then(response => (this.valueConverted = response.data))
        .catch(error => {});
    },
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
      }
      CustomizedProductRequests.getCustomizedProduct(store.state.customizedProduct.id)
      .then(response => {
              if(response.data.slots.length > 1){
                this.$emit("advance");
              }else{
                CustomizedProductRequests.createRecommendedSlots(store.state.customizedProduct.id)
                .then(response => {
                  this.$emit("advance");
                })
              .catch((error_message) => {
              });
              }
            })
             .catch((error_message) => {
          });
    },
    postSlots(){
       let slotsToPost1 = [];
      for(let i = 0; i< this.slotsToPost.length - 1; i++){
        slotsToPost1.unshift(this.slotsToPost[i]);
      }
      return new Promise((accept,reject)=>{
        this.postSlot(slotsToPost1.slice())
        .then((customizedProduct) => {
          for(let i = 0; i< this.slotsToPost.length; i++){
              store.dispatch(SET_ID_SLOT, {
                idSlot: customizedProduct.slots[i].id,
                position: i});
          }
          accept(customizedProduct)
        })
        .catch((error_message) => { 
                reject(error_message)
        });
      })
    },
    postSlot(slotsToPost1){
      return new Promise((accept, reject) => {
        let slotToPost = slotsToPost1.pop();
        CustomizedProductRequests.postCustomizedProductSlot(store.state.customizedProduct.id,
              {
                height: slotToPost.height,
                depth: slotToPost.depth,
                width: slotToPost.width,
                unit: slotToPost.unit
              }).then((customizedProduct) => {
                if(slotsToPost1.length > 0 ){
                  return this.postSlot(slotsToPost1)
                  .then((customizedProduct)=>{
                    accept(customizedProduct)})
                  .catch((error_message) => { reject(error_message)});
                }else{
                  accept(customizedProduct.data);
                }
              })
              .catch((error_message) => {
                  reject(error_message.response.data.message);
              });
      })
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
            store.dispatch(ADD_SLOT_DIMENSIONS); 
            this.$emit("back");
          }
        })
    },
    activateCanvasControls(){
      store.dispatch(ADD_SLOT_DIMENSIONS);
      this.getMinSlots();
      store.dispatch(ACTIVATE_CAN_MOVE_SLOTS);
    },
    deactivateCanvasControls(){
      store.dispatch(ADD_SLOT_DIMENSIONS);
      this.getRecommendedSlots()
      store.dispatch(DEACTIVATE_CAN_MOVE_SLOTS)
    },
    getRecommendedSlots(){
       this.listRecommendedSlots = [];
       CustomizedProductRequests.getCustomizedProductRecommendedSlots(store.state.customizedProduct.id)
            .then(response => {
              this.listRecommendedSlots = response.data;
              this.drawRecommendedSlots();
          })
          .catch((error_message) => {
              this.$toast.open("There was an error get recommended slots. Please try again!");
          });
    },
    drawRecommendedSlots(){
       this.slotsToPost = [];
      var depthCloset = store.state.customizedProduct.customizedDimensions.depth;
      var heightCloset = store.state.customizedProduct.customizedDimensions.height;
      var unitCloset = store.state.customizedProduct.customizedDimensions.unit;
      var reasonW = store.state.resizeVectorGlobal.width;
        for (let i = 0; i < this.listRecommendedSlots.length; i++) {
            store.dispatch(ADD_SLOT_DIMENSIONS, {
              idSlot: i,
              width: this.listRecommendedSlots[i].width * reasonW,
              height: heightCloset,
              depth: depthCloset,
              unit: unitCloset
            });
            CustomizedProductRequests.createRecommendedSlots(store.state.customizedProduct.id)
            .then(response => {
            })
             .catch((error_message) => {
          });
        } 
    },
    getMinSlots(){
        CustomizedProductRequests.getCustomizedProductMinimumSlots(store.state.customizedProduct.id)
            .then(response => {
              this.slotsToPost = response.data;
              this.drawMinSlots();
              this.postSlots();
              return this.slotsToPost.length;
            })
            .catch((error_message) => {
              this.$toast.open({
                message: error_message.response.data.message
              });
            });
    },
    drawMinSlots(){

      this.lines =  [];
      this.sliderValues = [];
              var widthCloset = store.state.customizedProduct.customizedDimensions.width;
              var depthCloset = store.state.customizedProduct.customizedDimensions.depth;
              var heightCloset = store.state.customizedProduct.customizedDimensions.height;
              
              var unitCloset = store.state.customizedProduct.customizedDimensions.unit;
             
             var reasonW = store.state.resizeVectorGlobal.width;

              for (let i = 0; i < this.slotsToPost.length; i++) {
                store.dispatch(ADD_SLOT_DIMENSIONS, {
                  idSlot: i,
                  width: this.slotsToPost[i].width * reasonW,
                  height: heightCloset,
                  depth: depthCloset,
                  unit: unitCloset
                });
                if(i < this.slotsToPost.length - 1){
                  this.lines.push({
                  slider: null
                  }) 
                  this.sliderValues[i] = this.maxSizeSlot
                  CustomizedProductRequests.postCustomizedProductSlot(store.state.customizedProduct.id,
                  {
                    height: heightCloset,
                    depth: depthCloset,
                    width: this.slotsToPost[i].width,
                    unit: unitCloset

                  })
                  .then(response => {
                  })
                  .catch((error_message) => {
                  });
                }
              }
    },
    drawOneSlot(){

      this.sliderValues = [];

      var reasonW = store.state.resizeVectorGlobal.width;
              CustomizedProductRequests.postCustomizedProductSlot(store.state.customizedProduct.id,
              {
                height: store.state.customizedProduct.customizedDimensions.height,
                depth:  store.state.customizedProduct.customizedDimensions.depth,
                width: store.getters.minSlotWidth,
                unit:  store.state.customizedProduct.customizedDimensions.unit,
              }).then(response => {
                
                    this.drawCustomizedSlots = response.data.slots

                    store.dispatch(ADD_SLOT_DIMENSIONS);
             
             for(let i=0; i<this.drawCustomizedSlots.length; i++){
               store.dispatch(ADD_SLOT_DIMENSIONS, {
                  idSlot: this.drawCustomizedSlots[i].id,
                  width: this.drawCustomizedSlots[i].dimensions.width * reasonW,
                  height: this.drawCustomizedSlots[i].dimensions.height,
                  depth: this.drawCustomizedSlots[i].dimensions.depth,
                  unit: this.drawCustomizedSlots[i].dimensions.unit,
               })
             }
                })
              .catch((error_message) => {
                 this.$toast.open("Imposiivel desehar mais slots");

              });
             
                  
    },
    removeOneSlot(){
     var reasonW = store.state.resizeVectorGlobal.width;
      CustomizedProductRequests.deleteCustomizedProductSlot(store.state.customizedProduct.id,this.drawCustomizedSlots[this.drawCustomizedSlots.length-1].id)
      .then(response => {
             this.drawCustomizedSlots.pop();
              store.dispatch(ADD_SLOT_DIMENSIONS);
      for(let i=0; i<this.drawCustomizedSlots.length; i++){
        store.dispatch(ADD_SLOT_DIMENSIONS, {
                  idSlot: this.drawCustomizedSlots[i].id,
                  width: this.drawCustomizedSlots[i].dimensions.width * reasonW,
                  height: this.drawCustomizedSlots[i].dimensions.height,
                  depth: this.drawCustomizedSlots[i].dimensions.depth,
                  unit: this.drawCustomizedSlots[i].dimensions.unit,
                });
      }
        })
        .catch((error_message) => {this.$toast.open("Imposiivel remover mais slots");});
       
    },
    updateWidthSlot(index){
      var depthCloset = 0;
      var heightCloset = 0;
      var unitCloset = store.state.customizedProduct.customizedDimensions.unit; 
      var reasonW = store.state.resizeVectorGlobal.width;
      store.dispatch(ADD_SLOT_DIMENSIONS, {
                  idSlot: index,
                  width: this.sliderValues[index] * reasonW,
                  height: heightCloset,
                  depth: depthCloset,
                  unit: unitCloset
      });        
    },
    putWidthSlot(index){
      
     var reasonW = store.state.resizeVectorGlobal.width;
     var value = 400;

      var updateSliderValues = this.sliderValues;
      var custProducSlots = [];//this.drawCustomizedSlots[index].id; 
       


       
        CustomizedProductRequests.getCustomizedProduct(store.state.customizedProduct.id)
        .then((response) => {
          custProducSlots = response.data.slots;
          CustomizedProductRequests.putCustomizedProductSlot(store.state.customizedProduct.id,custProducSlots[index].id, 
          {
            dimensions: {
              height: custProducSlots[index].dimensions.height,
              width: this.sliderValues[index],
              depth: custProducSlots[index].dimensions.depth,
              unit: custProducSlots[index].dimensions.unit,
            }
          })
          .then((custProduct) => {
            value=custProduct.data.slots[index].dimensions.width
          })
          .catch((error_message) => {
            this.lines.splice(index, 1);
            this.lines.push({
              slider:value
            })
            alert(value)
            this.sliderValues[index] = value;
            alert(this.sliderValues[index]);
          })
        })
        .catch((error_message) => {
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
      this.createNewSlider = true;
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