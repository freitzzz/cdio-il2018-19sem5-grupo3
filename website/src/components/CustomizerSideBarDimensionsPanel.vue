<template>
    <div>
      <!--Fetch minimums from server-->
      <input type="number" min="1" name="height" v-model="height">
      <input type="number" min="1" name="width" v-model="width">
      <input type="number" min="1" name="depth" v-model="depth">
      <select>
        <option value="mm">Milimeters</option>
        <option value="cm">Centimeters</option>
        <option value="dm">Decimeters</option>
        <option value="m">Meters</option>
      </select>
            {{$store.state}}

    </div>
</template>

<script>
export default {
  name: "CustomizerSideBarDimensionsPanel",
  data() {
    return {
      //TODO: replace hardcoded values ASAP
      height: 50,
      width: 409,
      depth: 100
    };
  }
};
</script>

