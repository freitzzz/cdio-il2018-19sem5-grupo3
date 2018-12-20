<template>
  <ul class="progressbar">
      <li v-for="stage in stages" :key="stage.title" :class="stage.class">
        {{stage.title}}
      </li>
  </ul>
</template>
<script>
export default {
  name: "CustomizerProgressBar",
  data() {
    return {
      currentStageIndex: 0,
      stages: [
        {
          title: "Structure",
          class: "active"
        },
        {
          title: "Dimensions",
          class: "before"
        },
        {
          title: "Divisions",
          class: "before"
        },
        {
          title: "Materials",
          class: "before"
        },
        {
          title: "Components",
          class: "before"
        },
        {
          title: "Payment",
          class: "before"
        }
      ]
    };
  },
  props: {
    stageIndex: 0
  },
  watch: {
    stageIndex: function(){
      if(this.currentStageIndex > this.stageIndex){
        this.currentStageIndex = this.stageIndex;
        this.previousStage()
      }else{
        this.currentStageIndex = this.stageIndex;
        this.nextStage()
      }
    }
  },
  methods: {
     /**
       * Go to the previous stage.
       */
      previousStage() {
        if (this.currentStageIndex >= 0) {
          this.stages[this.currentStageIndex + 1].class = "before"
        }
      },
      /**
       * Go to the next stage.
       */
      nextStage() {
        if (this.currentStageIndex < this.stages.length - 1) {
          this.stages[this.currentStageIndex].class = "active";
        }
      }
  }
};
</script>

<style>
.progressbar {
  counter-reset: step;
  padding-bottom: 4.8%; 
  padding-top: 1.2%;
}
.progressbar li {
  list-style-type: none;
  height: 10%;
  width: 15%;
  float: left;
  font-size: 12px;
  position: relative;
  text-align: center;
  text-transform: uppercase;
  color: #7d7d7d;
}
.progressbar li:before {
  width: 30px;
  height: 30px;
  content: counter(step);
  counter-increment: step;
  line-height: 30px;
  border: 2px solid #7d7d7d;
  display: block;
  text-align: center;
  margin: 0 auto 10px auto;
  border-radius: 50%;
  background-color: white;
}
.progressbar li:after {
  width: 90%;
  height: 2px;
  content: "";
  position: absolute;
  background-color: #7d7d7d;
  top: 15px;
  left: -45%;
  z-index: 1;
}
.progressbar li:first-child:after {
  content: none;
}
.progressbar li.active {
  color: #0ba2db;
}
.progressbar li.active:before {
  border-color: #0ba2db;
}
.progressbar li.active + li:after {
  background-color: #0ba2db;
}
</style>

