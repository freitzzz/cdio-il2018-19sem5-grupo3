<template>
<div>
  
</div>

  <!-- <div class="row">
      <div class="col-75">
        <div class="container">
          <form action="/action_page.php">
    
            <div class="row">
              <div class="col-50">
                <h3>Billing Address</h3>
                <label for="fname"><i class="fa fa-user"></i> Full Name</label>
                <input type="text" id="fname" name="firstname" placeholder="John M. Doe">
                <label for="email"><i class="fa fa-envelope"></i> Email</label>
                <input type="text" id="email" name="email" placeholder="john@example.com">
                <label for="adr"> Address</label>
                <input type="text" id="adr" name="address" placeholder="542 W. 15th Street">
                <label for="city"><!-City</label>
                <input type="text" id="city" name="city" placeholder="New York">
                <label for="zip">Zip</label>
                <input type="text" id="zip" name="zip" placeholder="10001">
    
    
              </div>
    
              <div class="col-50">
    
                <label for="cname">Name on Card</label>
                <input type="text" id="cname" name="cardname" placeholder="John More Doe">
                <label for="ccnum">Credit card number</label>
                <input type="text" id="ccnum" name="cardnumber" placeholder="1111-2222-3333-4444">
                <label for="expmonth">Exp Month</label>
                <input type="text" id="expmonth" name="expmonth" placeholder="September">
    
                <div class="row">
                  <div class="col-50">
                    <label for="expyear">Exp Year</label>
                    <input type="text" id="expyear" name="expyear" placeholder="2018">
                  </div>
                  <div class="col-50">
                    <label for="cvv">CVV</label>
                    <input type="text" id="cvv" name="cvv" placeholder="352">
                  </div>
                </div>
              </div>
    
            </div>
    
          </form>
    
   
        </div>
       
      </div>
    
      <div class="col-25">
        <div class="container">
          <h4>Cart
            <span class="price" style="color:black">
                  <i class="fa fa-shopping-cart"></i>
                  <b>4</b>
                </span>
          </h4>
          <p><a>Pole By Stephano</a> <span class="price">$499.99</span></p>
          <p><a>Drawer</a> <span class="price">$5</span></p>
          <p><a>Closet</a> <span class="price">$5</span></p>
          <p><a>Matte Finish</a> <span class="price">$1.99</span></p>
          <hr>
          <p>Total <span class="price" style="color:black"><b>$511.98</b></span></p>
        </div>
            <input type="checkbox" name="checkbox" value="check" id="agree" /> I have read and agree to the Terms and Conditions and Privacy Policy
          <p></p>
          <i class="btn btn-primary">Continue to checkout</i>
  
      </div>
    </div> -->
</template>

<script>
  import Vue from 'vue';
  /* import { library } from '@fortawesome/fontawesome-svg-core';
  import { faCoffee } from '@fortawesome/free-solid-svg-icons';
  import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'; */
  import store from "./../../store";
  import {
    DEACTIVATE_CAN_MOVE_COMPONENTS
  } from "./../../store/mutation-types.js";
  
  export default {
    name: "CustomizerCheckOut",
    created() {
      store.dispatch(DEACTIVATE_CAN_MOVE_COMPONENTS);
    }
  }
</script>

<style>
  .row {
    display: -ms-flexbox;
    /* IE10 */
    display: flex;
    -ms-flex-wrap: wrap;
    /* IE10 */
    flex-wrap: wrap;
    margin: 13px;
    font-size: 10px;
    height: 70%;
  }
  
  .col-25 {
    -ms-flex: 25%;
    /* IE10 */
    flex: 25%;
  }
  
  .col-50 {
    -ms-flex: 50%;
    /* IE10 */
    flex: 50%;
  }
  
  .col-75 {
    -ms-flex: 75%;
    /* IE10 */
    flex: 70%;
  }
  
  .col-25,
  .col-50,
  .col-75 {
    padding: 0 5px;
  }
  
  .container {
    background-color: #f2f2f2;
    padding: 5px 20px 15px 20px;
    border: 1px solid lightgrey;
    border-radius: 3px;
    width: 100%;
  }
  
  input[type=text] {
    width: 100%;
    margin-bottom: 5px;
    padding: 12px;
    border: 1px solid #ccc;
    border-radius: 3px;
  }
  
  label {
    margin-bottom: 10px;
    display: block;
  }
  
  .icon-container {
    margin-bottom: 20px;
    padding: 7px 0;
  }
  
  .btn {
    background-color: #0ba2db;
    color: white;
    margin: 10px 0;
    border-radius: 3px;
  }
  
  .btn:hover {
    background-color: #0ba2db;
  }
  
  span.price {
    float: right;
    color: grey;
  }
  
  
  /* Responsive layout - when the screen is less than 800px wide, make the two columns stack on top of each other instead of next to each other (and change the direction - make the "cart" column go on top) */
  
  @media (max-width: 300px) {
    .row {
      flex-direction: column-reverse;
    }
    .col-25 {
      margin-bottom: 20px;
    }
  }
</style>
