using System.Collections.Generic;
using support.domain.ddd;
using System;
using support.dto;
using core.dto;
using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using support.domain;

namespace core.domain {
    /// <summary>
    /// Class that represents a Customized Product.
    /// </summary>
    public class CustomizedProduct : Activatable, AggregateRoot<string>, DTOAble<CustomizedProductDTO> {

        /// <summary>
        /// Constant that represents the message that occurs if the CustomizedProduct's material is not valid
        /// </summary>
        private const string INVALID_CUSTOMIZED_PRODUCT_MATERIAL = "The chosen material is not valid.";

        /// <summary>
        /// Constant that represents the message that occurs if the CustomizedProduct's dimensions are not valid
        /// </summary>
        private const string INVALID_CUSTOMIZED_PRODUCT_DIMENSIONS = "The dimensions are not valid.";

        /// <summary>
        /// Constant that represents the message presented if provided CustomizedProduct's dimensions do not match any of Product's Measurements.
        /// </summary>
        private const string CUSTOMIZED_PRODUCT_DIMENSIONS_NOT_MATCHING_SPECIFICATION = "The dimensions don't match the product's specification.";

        /// <summary>
        /// Constant that represents the message that occurs if the CustomizedProduct's reference is not valid
        /// </summary>
        private const string INVALID_REFERENCE = "The reference is not valid.";

        /// <summary>
        /// Constant that represents the message presented if the CustomizedProduct's user authentication token is not valid.
        /// </summary>
        private const string INVALID_AUTH_TOKEN = "The authentication token is not valid.";

        /// <summary>
        /// Constant that represents the message that occurs if the CustomizedProduct's product is not valid
        /// </summary>
        private const string INVALID_PRODUCT = "The product is not valid.";

        /// <summary>
        /// Constant that represents the message that occurs if the CustomizedProduct's designation is not valid
        /// </summary>
        private const string INVALID_DESIGNATION = "The designation is not valid.";

        /// <summary>
        /// Constant that represents the message presented when a CustomizedProduct is attempted to be added to a null CustomizedProduct.
        /// </summary>
        private const string ADD_CUSTOMIZED_PRODUCT_TO_NULL_PARENT = "Unable to add the customized product to a null customized product.";

        /// <summary>
        /// Constant that represents the message presented when a CustomizedProduct is attempted to be added to a null slot.
        /// </summary>
        private const string ADD_CUSTOMIZED_PRODUCT_TO_NULL_SLOT = "Unable to add the customized product to a null slot.";

        /// <summary>
        /// Constant that represents the message presented when a null Slot is attempted to be resized.
        /// </summary>
        private const string RESIZE_NULL_SLOT = "Unable to resize null slot.";

        /// <summary>
        /// Constant that represents the message presented when a null Slot is attempted to be removed.
        /// </summary>
        private const string REMOVE_NULL_SLOT = "Unable to remove null slot.";

        /// <summary>
        /// Constant that represents the message that if a Slot with null CustomizedDimensions is attempted to be added.
        /// </summary>
        private const string NULL_SLOT_DIMENSIONS = "Unable to add a slot with null dimensions.";

        /// <summary>
        /// Constant that represents the message that occurs if the CustomizedProduct's product doesn't support slots
        /// </summary>
        private const string PRODUCT_DOES_NOT_SUPPORT_SLOTS = "The product doesn't support slots";

        /// <summary>
        /// Constant that represents the message when a sub CustomizedProduct's reference is attempted to be changed.
        /// </summary>
        private const string CHANGE_CHILD_CUSTOMIZED_PRODUCT_REFERENCE = "A sub customized procut is not allowed to change its reference.";

        /// <summary>
        /// Constant that represents the message when a sub CustomizedProduct's designation is attempted to be changed.
        /// </summary>
        private const string CHANGE_CHILD_CUSTOMIZED_PRODUCT_DESIGNATION = "A sub customized product is not allowed to change its designation.";

        /// <summary>
        /// Constant that represents the message presented when the CustomizedProduct's dimensions are attempted to be changed after adding slots.
        /// </summary>
        private const string CHANGE_DIMENSIONS_AFTER_ADDING_SLOTS = "Unable to change dimensions after adding slots.";

        /// <summary>
        /// Constant that represents the messsage presented when the CustomizedProduct's finish is attempted to be changed before defining the CustomizedMaterial.
        /// </summary>
        private const string CHANGE_FINISH_BEFORE_DEFINING_MATERIAL = "Unable to change the finish before defining the material.";

        /// <summary>
        /// Constant that represents the message presented when the CustomizedProduct's color is attempted to be changed before defining the CustomizedMaterial.
        /// </summary>
        private const string CHANGE_COLOR_BEFORE_DEFINING_MATERIAL = "Unable to change the color before defining the material.";

        /// <summary>
        /// Constant that represents the message presented when a Slot is attempted to be added before setting the CustomizedProduct's dimensions.
        /// </summary>
        private const string ADD_SLOT_AFTER_ADDING_CUSTOMIZED_PRODUCTS = "Unable to add slots after having added customized products.";

        /// <summary>
        /// Constant that represents the message presented when a Slot is attempted to be created with Dimensions not allowed by the Product's specification.
        /// </summary>
        private const string SLOT_DIMENSIONS_NOT_RESPECTING_SPECIFICATION = "The slot's dimensions do not follow the specification.";

        /// <summary>
        /// Constant that represents the message presented when a Slot is attempted to be created with Dimensions greater than the CustomizedProduct's.
        /// </summary>
        private const string SLOT_LARGER_THAN_CUSTOMIZED_PRODUCT = "The slot's dimensions are larger than the customized product's.";

        /// <summary>
        /// Constant that represents the message presented when a Slot being added does not fit.
        /// </summary>
        private const string SLOT_DOES_NOT_FIT = "The slot does not fit.";

        /// <summary>
        /// Constant that represents the message presented when a Slot is not contained in this CustomizedProduct's collection of Slot.
        /// </summary>
        private const string SLOT_NOT_FOUND = "Unable to find the given slot.";

        /// <summary>
        /// Constant that represents the message presented when the only Slot is attempted to be removed.
        /// </summary>
        private const string REMOVE_LAST_SLOT = "Removing the only slot is not allowed.";

        /// <summary>
        /// Constant that represents the message presented when the only Slot is attempted to be removed.
        /// </summary>
        private const string RESIZE_ONLY_SLOT = "Resizing the only slot in the customized product is not allowed.";

        /// <summary>
        /// Constant that represents the error message presented when a null CustomizedProduct is attempted to be added.
        /// </summary>
        private const string ADD_NULL_CUSTOMIZED_PRODUCT = "Unable to add a null customized product.";

        /// <summary>
        /// Constant that represents the error message presented when a null CustomizedProduct is attempted to be removed.
        /// </summary>
        private const string REMOVE_NULL_CUSTOMIZED_PRODUCT = "Unable to remove a null customized product.";

        /// <summary>
        /// Constant that represents the error message presented when the CustomizedProduct being added does not match any of the available components.
        /// </summary>
        private const string CUSTOMIZED_PRODUCT_DOES_NOT_MATCH_CHILDREN = "The product is not a valid component.";

        /// <summary>
        /// Constant that represents the error message presented when a child CustomizedProduct attempts to start finalizing customization.
        /// </summary>
        private const string CHILD_STARTING_FINALIZATION = "A child can't finalize the customization process.";

        /// <summary>
        /// Constant that represents the error message presented when attempting to finalize customization without mandatory components.
        /// </summary>
        private const string FINALIZING_WITHOUT_MANDATORY_COMPONENTS = "Unable to finalize customization without mandatory components.";

        /// <summary>
        /// Constant that represents the error message presented when attempting to finalize customization without material
        /// </summary>
        private const string FINALIZING_WITHOUT_MATERIAL = "Unable to finalize customization without material.";

        /// <summary>
        /// Constant that represents the message presented when any action is attempted to be performed after the customization is finished.
        /// </summary>
        private const string ACTION_AFTER_CUSTOMIZATION_FINISHED = "The current customization's status does not allow further customization.";

        /// <summary>
        /// Constant that represents the message presented when an instance of Slot could not be resized.
        /// </summary>
        private const string UNABLE_TO_RESIZE_SLOT = "Unable to resize the slot, since it would invalidate other slots.";

        /// <summary>
        /// Constant that represents the message presented when trying to add a customized product with and invalid finish
        /// </summary>
        private const string CHILD_FINISH_INAVLID = "The Product's finish does not fulfill the restrictions!";

        /// <summary>
        /// Constant that represents the message presented when trying to add a customized product with and invalid color
        /// </summary>
        private const string CHILD_COLOR_INVALID = "The Product's color does not fulfill the restrictions!";

        /// <summary>
        /// Constant that represents the message presented when trying to add a customized product with and invalid material
        /// </summary>
        private const string CHILD_MATERIAL_INVALID = "The Product's material does not fulfill the restrictions!";

        /// <summary>
        /// Constant that represents the message presented when trying to add a customized product with and invalid dimension
        /// </summary>
        private const string CHILD_DIMENSION_INVALID = "The Product's dimension does not fulfill the restrictions!";

        /// <summary>
        /// Constant that represents the message presented when trying to add a customized product with a null customized material
        /// </summary>
        private const string ADD_NULL_CUSTOMIZED_MATERIAL = "The Product does not have a customized material!";

        /// Constant representing the string used for delimiting CustomizedProduct references.
        /// </summary>
        private const string REFERENCE_DELIMITER = "-CP";

        /// <summary>
        /// Constant representing the string used for delimiting Slot identifiers.
        /// </summary>
        private const string SLOT_IDENTIFIER_DELIMITER = "-S";

        /// <summary>
        /// Long that represents the CustomizedProduct's persistence ID.
        /// </summary>
        public long Id { get; internal set; }

        /// <summary>
        /// String with the CustomizedProduct's reference
        /// </summary>
        public string reference { get; protected set; }

        /// <summary>
        /// String with the CustomizedProduct's designation
        /// </summary>
        public string designation { get; protected set; }

        /// <summary>
        /// Authentication token of the user who created the CustomizedProduct.
        /// </summary>
        /// <value>Gets/protected sets the authentication token.</value>
        public string authToken { get; protected set; }

        /// <summary>
        /// CustomizedProduct's CustomizationStatus.
        /// </summary>
        /// <value>Gets/sets the CustomizationStatus</value>
        public CustomizationStatus status { get; protected set; }

        /// <summary>
        /// CustomizedMaterial that represents the CustomizedProduct's material
        /// </summary>
        private CustomizedMaterial _customizedMaterial; //!private field used for lazy loading, do not use this for storing or fetching data
        public CustomizedMaterial customizedMaterial { get => LazyLoader.Load(this, ref _customizedMaterial); protected set => _customizedMaterial = value; }


        /// <summary>
        /// CustomizedDimensions that represents the CustomizedProduct's dimensions
        /// </summary>
        private CustomizedDimensions _customizedDimensions; //!private field used for lazy loading, do not use this for storing or fetching data
        public CustomizedDimensions customizedDimensions { get => LazyLoader.Load(this, ref _customizedDimensions); protected set => _customizedDimensions = value; }

        /// <summary>
        /// Product that represents the product that the CustomizedProduct refers to
        /// </summary>
        /// <value></value>
        private Product _product;   //!private field used for lazy loading, do not use this for storing or fetching data
        public Product product { get => LazyLoader.Load(this, ref _product); protected set => _product = value; }

        /// <summary>
        /// List of Slots that the CustomizedProduct has
        /// </summary>
        private List<Slot> _slots;  //!private field used for lazy loading, do not use this for storing or fetching data
        public List<Slot> slots { get => LazyLoader.Load(this, ref _slots); protected set => _slots = value; }

        /// <summary>
        /// Identifier of the slot that this customized product belongs to
        /// </summary>
        /// <value>Get/Internal set of the identifier</value>
        public long? insertedInSlotId { get; internal set; }

        /// <summary>
        /// Slot that this customized product belongs to
        /// </summary>
        private Slot _insertedInSlot; //!private field used for lazy loading, do not use this for storing or fetching data 

        public Slot insertedInSlot { get => LazyLoader.Load(this, ref _insertedInSlot); protected set => _insertedInSlot = value; }

        /// <summary>
        /// LazyLoader being injected by the framework.
        /// </summary>
        /// <value>Private Gets/Sets the value of the LazyLoader.</value>
        private ILazyLoader LazyLoader { get; set; }

        /// <summary>
        /// Private constructor used for injecting the LazyLoader.
        /// </summary>
        /// <param name="lazyLoader">LazyLoader being injected.</param>
        private CustomizedProduct(ILazyLoader lazyLoader) {
            this.LazyLoader = lazyLoader;
        }

        /// <summary>
        /// Empty constructor for ORM.
        /// </summary>
        protected CustomizedProduct() { }

        /// <summary>
        ///  Creates a new instance of CustomizedProduct with a given reference, based on the specifications of a given Product.
        /// </summary>
        /// <param name="reference">Reference assigned to this CustomizedProduct.</param>
        /// <param name="product">Product defining the specification for this CustomizedProduct.</param>
        /// <param name="customizedDimensions">Instance of CustomizedDimensions detailing the CustomizedProduct's dimensions.</param>
        private CustomizedProduct(string reference, Product product, CustomizedDimensions customizedDimensions) {
            checkString(reference, INVALID_REFERENCE);
            checkProduct(product);
            checkCustomizedDimensions(customizedDimensions, product);
            this.reference = reference;
            this.product = product;
            this.customizedDimensions = customizedDimensions;
            this.status = CustomizationStatus.PENDING;
            this.slots = new List<Slot>();

            //Add slot matching the CustomizedProduct's dimensions
            string slotIdentifier = buildSlotIdentifier(this);
            this.slots.Add(new Slot(slotIdentifier, customizedDimensions));
        }

        /// <summary>
        /// Creates a new instance of CustomizedProduct with a given reference, based on the specifications of a given Product,
        /// with the given authentication token.
        /// </summary>
        /// <param name="authToken">Authentication token of the user creating this CustomizedProduct.</param>
        /// <param name="reference">Reference assigned to this CustomizedProduct.</param>
        /// <param name="product">Product defining the specification for this CustomizedProduct.</param>
        /// <param name="customizedDimensions">Instance of CustomizedDimensions detailing the CustomizedProduct's dimensions.</param>
        private CustomizedProduct(string authToken, string reference, Product product, CustomizedDimensions customizedDimensions)
            : this(reference, product, customizedDimensions) {
            checkString(authToken, INVALID_AUTH_TOKEN);
            this.authToken = authToken;
        }


        /// <summary>
        /// Creates a new instance of CustomizedProduct with a given reference, based on the specifications of a given Product 
        /// and adds it to the specified Slot in the specified parent CustomizedProduct.
        /// </summary>
        /// <param name="product">Product defining the specification for this CustomizedProduct.</param>
        /// <param name="customizedDimensions">Instance of CustomizedDimensions detailing the CustomizedProduct's dimensions.</param>

        /// <param name="parentCustomizedProduct">CustomizedProduct to which the new CustomizedProduct will be added.</param>
        /// <param name="insertedInSlot">Slot in which the new CustomizedProduct will be inserted.</param>
        private CustomizedProduct(Product product, CustomizedDimensions customizedDimensions, CustomizedProduct parentCustomizedProduct, Slot insertedInSlot) {
            checkProduct(product);
            checkCustomizedDimensions(customizedDimensions, product);
            checkParentCustomizedProduct(parentCustomizedProduct);
            checkInsertedInSlot(insertedInSlot);

            this.reference = buildSubCustomizedProductReference(parentCustomizedProduct);
            this.designation = parentCustomizedProduct.designation;
            this.product = product;
            this.customizedDimensions = customizedDimensions;
            this.status = CustomizationStatus.PENDING;
            this.slots = new List<Slot>();

            //Add slot matching the CustomizedProduct's dimensions
            string slotIdentifier = buildSlotIdentifier(this);
            this.slots.Add(new Slot(slotIdentifier, customizedDimensions));

            //add it to the parent
            parentCustomizedProduct.addCustomizedProduct(this, insertedInSlot);
        }


        /// <summary>
        /// Creates a new instance of CustomizedProduct with a given reference, based on the specifications of a given Product 
        /// with the given authentication token and adds it to the specified Slot in the specified parent CustomizedProduct. 
        /// </summary>
        /// <param name="authToken">Authentication token of the user creating this CustomizedProduct.</param>
        /// <param name="product">Product defining the specification for this CustomizedProduct.</param>
        /// <param name="customizedDimensions">Instance of CustomizedDimensions detailing the CustomizedProduct's dimensions.</param>
        /// <param name="parentCustomizedProduct">CustomizedProduct to which the new CustomizedProduct will be added.</param>
        /// <param name="insertedInSlot">Slot in which the new CustomizedProduct will be inserted.</param>
        private CustomizedProduct(string authToken, Product product, CustomizedDimensions customizedDimensions, CustomizedProduct parentCustomizedProduct, Slot insertedInSlot)
            : this(product, customizedDimensions, parentCustomizedProduct, insertedInSlot) {
            checkString(authToken, INVALID_AUTH_TOKEN);
            this.authToken = authToken;
        }

        /// <summary>
        /// Changes the CustomizedProduct's reference.
        /// </summary>
        /// <param name="reference">New reference.</param>
        ///<exception cref="System.InvalidOperationException">
        /// Thrown when the CustomizedProduct's customization is finished or when a sub CustomizedProduct attempts to change reference.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the provided reference is null or empty.
        /// </exception>
        public void changeReference(string reference) {
            if (this.status == CustomizationStatus.FINISHED) throw new InvalidOperationException(ACTION_AFTER_CUSTOMIZATION_FINISHED);
            //only base customized products should be allowed to change reference
            if (this.insertedInSlot != null) throw new InvalidOperationException(CHANGE_CHILD_CUSTOMIZED_PRODUCT_REFERENCE);
            checkString(reference, INVALID_REFERENCE);

            string previousReference = this.reference;
            this.reference = reference;

            changeReferenceRec(previousReference, reference);
        }


        /// <summary>
        /// Recursively changes the reference for all the sub CustomizedProducts.
        /// </summary>
        /// <param name="previousReference">Previous reference.</param>
        /// <param name="reference">New reference.</param>
        private void changeReferenceRec(string previousReference, string reference) {
            foreach (Slot slot in this.slots) {
                string previousSlotIdentifier = slot.identifier;

                string newSlotIdentifier = slot.identifier.Replace(previousReference, reference);

                slot.changeIdentifier(newSlotIdentifier);

                foreach (CustomizedProduct subCustomizedProduct in slot.customizedProducts) {
                    string previousSubCustomizedProductReference = subCustomizedProduct.reference;

                    string newSubCustomizedProductReference = subCustomizedProduct.reference.Replace(previousReference, reference);

                    subCustomizedProduct.reference = newSubCustomizedProductReference;

                    subCustomizedProduct.changeReferenceRec(previousReference, reference);
                }
            }
        }

        /// <summary>
        /// Changes the CustomizedProduct's designation.
        /// </summary>
        /// <param name="designation">New designation.</param>
        public void changeDesignation(string designation) {
            if (this.status == CustomizationStatus.FINISHED) throw new InvalidOperationException(ACTION_AFTER_CUSTOMIZATION_FINISHED);
            //only base customized products should be allowed to change designation
            if (this.insertedInSlot != null) throw new InvalidOperationException(CHANGE_CHILD_CUSTOMIZED_PRODUCT_DESIGNATION);
            checkString(designation, INVALID_DESIGNATION);

            //recursively change the designation for all the children too
            changeDesignationRec(designation);
        }


        /// <summary>
        /// Recursively changes the designation for all the sub CustomizedProducts.
        /// </summary>
        /// <param name="designation">New designation.</param>
        private void changeDesignationRec(string designation) {
            this.designation = designation;

            foreach (Slot slot in this.slots) {
                foreach (CustomizedProduct subCustomizedProduct in slot.customizedProducts) {
                    subCustomizedProduct.changeDesignationRec(designation);
                }
            }
        }



        /// <summary>
        /// Changes the CustomizedProduct's dimensions.
        /// </summary>
        /// <param name="customizedDimensions">New customized dimensions</param>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when the CustomizedProduct's customization is finished or if slots have been added.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Thrown when the provided instance of CustomizedDimensions do not represent values available in the Product's collection of Measurement.
        /// <exception>
        public void changeDimensions(CustomizedDimensions customizedDimensions) {
            if (this.status == CustomizationStatus.FINISHED) throw new InvalidOperationException(ACTION_AFTER_CUSTOMIZATION_FINISHED);
            //check if any more slots have been added besides the one matching the CustomizedProduct's dimensions
            if (this.slots.Count > 1) throw new InvalidOperationException(CHANGE_DIMENSIONS_AFTER_ADDING_SLOTS);

            checkCustomizedDimensions(customizedDimensions, this.product);

            this.customizedDimensions = customizedDimensions;

            //resize the remaining slot to match the CustomizedProduct's dimensions
            Slot fullSizeSlot = this.slots.FirstOrDefault();
            fullSizeSlot.changeDimensions(customizedDimensions);
        }

        /// <summary>
        /// Changes the CustomizedProduct's customized material
        /// </summary>
        /// <param name="customizedMaterial">New customized material</param>
        public void changeCustomizedMaterial(CustomizedMaterial customizedMaterial) {
            if (this.status == CustomizationStatus.FINISHED) throw new InvalidOperationException(ACTION_AFTER_CUSTOMIZATION_FINISHED);
            checkCustomizedMaterial(customizedMaterial);
            this.customizedMaterial = customizedMaterial;
        }

        /// <summary>
        /// Changes the Finish of a CustomizedProduct's customized material
        /// </summary>
        /// <param name="finish">new finish</param>
        public void changeFinish(Finish finish) {
            if (this.status == CustomizationStatus.FINISHED) throw new InvalidOperationException(ACTION_AFTER_CUSTOMIZATION_FINISHED);
            if (this.customizedMaterial == null) throw new InvalidOperationException(CHANGE_FINISH_BEFORE_DEFINING_MATERIAL);
            //TODO: inform why it was not changed
            this.customizedMaterial.changeFinish(finish);
        }

        /// <summary>
        /// Changes the Color of a CustomizedProduct's customized material
        /// </summary>
        /// <param name="color">new color</param>
        public void changeColor(Color color) {
            if (this.status == CustomizationStatus.FINISHED) throw new InvalidOperationException(ACTION_AFTER_CUSTOMIZATION_FINISHED);
            if (this.customizedMaterial == null) throw new InvalidOperationException(CHANGE_COLOR_BEFORE_DEFINING_MATERIAL);
            //TODO: inform why it was not changed
            this.customizedMaterial.changeColor(color);
        }



        /// <summary>
        /// Adds a new slot to the CustomizedProduct's collection of Slot with the specified CustomizedDimensions.
        /// </summary>
        /// <param name="slotDimensions"></param>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when the CustomizedProduct's CustomizationStatus is Finished or when the Product does not allow support slots.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Thrown when the provided CustomizedDimensions are null, exceed the Product's ProductSlotWidths or is larger than the CustomizedProduct itself.
        /// </exception>
        public void addSlot(CustomizedDimensions slotDimensions) {
            if (this.status == CustomizationStatus.FINISHED) throw new InvalidOperationException(ACTION_AFTER_CUSTOMIZATION_FINISHED);

            if (!this.product.supportsSlots) throw new InvalidOperationException(PRODUCT_DOES_NOT_SUPPORT_SLOTS);

            if (slotDimensions == null) throw new ArgumentException(NULL_SLOT_DIMENSIONS);

            if (!isWithinProductSlotWidthsRange(slotDimensions.width)) throw new ArgumentException(SLOT_DIMENSIONS_NOT_RESPECTING_SPECIFICATION);

            if (!smallerThanCustomizedProduct(slotDimensions)) throw new ArgumentException(SLOT_LARGER_THAN_CUSTOMIZED_PRODUCT);

            if (hasCustomizedProducts()) throw new InvalidOperationException(ADD_SLOT_AFTER_ADDING_CUSTOMIZED_PRODUCTS);

            //check if there's only the slot matching the CustomizedProduct's dimensions
            if (this.slots.Count == 1) {
                //the initial slot must be resized
                Slot fullSizeSlot = this.slots.FirstOrDefault();

                double updatedWidth = fullSizeSlot.slotDimensions.width - slotDimensions.width;

                //if minimum width can't be respected throw exception
                if (!isWithinProductSlotWidthsRange(updatedWidth)) throw new ArgumentException(SLOT_DIMENSIONS_NOT_RESPECTING_SPECIFICATION);

                fullSizeSlot.changeDimensions(CustomizedDimensions.valueOf(customizedDimensions.height, updatedWidth, customizedDimensions.depth));
                string slotIdentifier = buildSlotIdentifier(this);

                slots.Add(new Slot(slotIdentifier, slotDimensions));
            } else {
                addSubsequentSlot(slotDimensions);
            }
        }

        /// <summary>
        /// Adds a new Slot after having added other Slots.
        /// </summary>
        /// <param name="slotDimensions">New Slot's CustomizedDimensions.</param>
        private void addSubsequentSlot(CustomizedDimensions slotDimensions) {
            double minPossibleWidth = this.product.slotWidths.minWidth;

            double idealSlotWidth = slotDimensions.width / (double)numberOfSlots();

            double availableWidth = 0;

            /*dictionary in which keys are slots and values are arrays with 2 positions 
            in which the first position is the slot's width and the second position is the value that can be expended*/
            Dictionary<Slot, double[]> slotDictionary = new Dictionary<Slot, double[]>();

            //build dictionary
            foreach (Slot slot in this.slots) {
                double maxExpendableValue = slot.slotDimensions.width - minPossibleWidth;
                slotDictionary.Add(slot, new double[] { slot.slotDimensions.width, maxExpendableValue });
            }

            Slot largestSlot = slotDictionary.OrderByDescending(order => order.Value[1]).Select(order => order.Key).FirstOrDefault();

            //if the largest slot can't give the ideal slot width
            double currentSlotWidth = slotDictionary[largestSlot][0];

            double slotWidthAfterSubtractingIdealSlotWidth = currentSlotWidth - idealSlotWidth;

            if (slotWidthAfterSubtractingIdealSlotWidth < minPossibleWidth) throw new ArgumentException(SLOT_DOES_NOT_FIT);

            //start attempting to add with the largest slot
            addSubsequentSlotRec(minPossibleWidth, idealSlotWidth, availableWidth, slotDimensions, largestSlot, slotDictionary);

            foreach (Slot slot in this.slots) {
                double oldHeight = slot.slotDimensions.height;
                double updatedWidth = slotDictionary[slot][0];
                double oldDepth = slot.slotDimensions.depth;

                CustomizedDimensions newDimensions = CustomizedDimensions.valueOf(oldHeight, updatedWidth, oldDepth);

                slot.changeDimensions(newDimensions);
            }

            string slotIdentifier = buildSlotIdentifier(this);

            Slot newSlot = new Slot(slotIdentifier, slotDimensions);

            slots.Add(newSlot);
        }

        /// <summary>
        /// Recursive method used for adding a Slot after slots have been added.
        /// </summary>
        /// <param name="minPossibleWidth">Slot's minimum possible width.</param>
        /// <param name="idealSlotWidth">Ideal slot's width.</param>
        /// <param name="availableWidth">Remaining available width.</param>
        /// <param name="newSlotDimensions">Instance of CustomizedDimensions detailing the Slot's dimension.</param>
        /// <param name="largestSlot">Largest Slot inserted in the CustomizedProduct.</param>
        /// <param name="slotDictionary">
        /// Dictionary in which the keys are CustomizedProducts and the values are arrays of double in which 
        /// the first position is the slot's width and the second position is the value that can be expended.
        /// </param>
        private void addSubsequentSlotRec(double minPossibleWidth, double idealSlotWidth, double availableWidth, CustomizedDimensions newSlotDimensions, Slot largestSlot,
            Dictionary<Slot, double[]> slotDictionary) {
            double currentSlotWidth = slotDictionary[largestSlot][0];

            double widthThatSlotCanExpended = idealSlotWidth;

            if (currentSlotWidth - idealSlotWidth < minPossibleWidth) {
                widthThatSlotCanExpended = currentSlotWidth - minPossibleWidth;
            }

            double slotWidthAfterSubtractingIdealSlotWidth = currentSlotWidth - widthThatSlotCanExpended;

            slotDictionary[largestSlot][0] = slotWidthAfterSubtractingIdealSlotWidth;
            slotDictionary[largestSlot][1] = slotWidthAfterSubtractingIdealSlotWidth - minPossibleWidth;

            availableWidth += widthThatSlotCanExpended;

            if (availableWidth == newSlotDimensions.width) {
                return;
            }

            largestSlot = slotDictionary.OrderByDescending(order => order.Value[1]).Select(order => order.Key).FirstOrDefault();

            //recursive method call
            addSubsequentSlotRec(minPossibleWidth, idealSlotWidth, availableWidth, newSlotDimensions, largestSlot, slotDictionary);
        }

        /// <summary>
        /// Removes a given Slot from the CustomizedProduct's Slot list
        /// </summary>
        /// <param name="slot">Slot to remove</param>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when the CustomizedProduct's CustomizationStatus is Finished or when the main slot is attempted to be removed.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Thrown when the provided Slot is null or has not been added to the CustomizedProduct.
        /// </exception>
        public void removeSlot(Slot slot) {
            if (this.status == CustomizationStatus.FINISHED) throw new InvalidOperationException(ACTION_AFTER_CUSTOMIZATION_FINISHED);

            if (slot == null) throw new ArgumentException(REMOVE_NULL_SLOT);

            Slot slotBeingRemoved = this.slots.Where(s => s.Equals(slot)).SingleOrDefault();

            if (slotBeingRemoved == null) throw new ArgumentException(SLOT_NOT_FOUND);

            if (this.slots.Count == 1) throw new InvalidOperationException(REMOVE_LAST_SLOT);

            if (hasCustomizedProducts()) throw new InvalidOperationException(ADD_SLOT_AFTER_ADDING_CUSTOMIZED_PRODUCTS);

            if (this.slots.Count == 2) {
                //the last slot has to match the product's dimensions
                this.slots.Remove(slotBeingRemoved);

                Slot mainSlot = this.slots.SingleOrDefault();

                mainSlot.changeDimensions(this.customizedDimensions);
            } else {
                double previousSlotWidth = slotBeingRemoved.slotDimensions.width;

                IDictionary<Slot, double> slotWidthMap = buildSlotLayoutDictionary(previousSlotWidth, 0, slotBeingRemoved, this.slots);

                foreach (KeyValuePair<Slot, double> entry in slotWidthMap) {
                    //remove the desired slot and resize all the others
                    if (entry.Key.Equals(slotBeingRemoved)) {
                        this.slots.Remove(slotBeingRemoved);
                    } else {
                        double height = entry.Key.slotDimensions.height;
                        double width = entry.Value;
                        double depth = entry.Key.slotDimensions.depth;

                        CustomizedDimensions resizedDimensions = CustomizedDimensions.valueOf(height, width, depth);

                        entry.Key.changeDimensions(resizedDimensions);
                    }
                }
            }
        }

        /// <summary>
        /// Resizes a given Slot.
        /// </summary>
        /// <param name="slot">Slot being resized.</param>
        /// <param name="newSlotDimensions">Slot's new CustomizedDimensions</param>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when the CustomizedProduct's CustomizationStatus is Finished or when the main slot is attempted to be resized.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Thrown when the provided Slot is null or has not been added to the CustomizedProduct.
        /// </exception>
        public void resizeSlot(Slot slot, CustomizedDimensions newSlotDimensions) {
            if (this.status == CustomizationStatus.FINISHED) throw new InvalidOperationException(ACTION_AFTER_CUSTOMIZATION_FINISHED);

            if (slot == null) throw new ArgumentException(RESIZE_NULL_SLOT);

            if (!isWithinProductSlotWidthsRange(newSlotDimensions.width)) throw new ArgumentException(SLOT_DIMENSIONS_NOT_RESPECTING_SPECIFICATION);

            Slot slotBeingResized = this.slots.Where(s => s.Equals(slot)).SingleOrDefault();

            if (slotBeingResized == null) throw new ArgumentException(SLOT_NOT_FOUND);

            if (this.slots.Count == 1) throw new InvalidOperationException(RESIZE_ONLY_SLOT);

            //skip doing all the other operations
            if (slotBeingResized.slotDimensions.Equals(newSlotDimensions)) return;

            IDictionary<Slot, double> slotWidthMap = buildSlotLayoutDictionary(slotBeingResized.slotDimensions.width, newSlotDimensions.width, slotBeingResized, this.slots);

            //resize all the slots with the values in the Dictionary
            foreach (KeyValuePair<Slot, double> entry in slotWidthMap) {
                double height = entry.Key.slotDimensions.height;
                double width = entry.Value;
                double depth = entry.Key.slotDimensions.depth;

                CustomizedDimensions resizedDimensions = CustomizedDimensions.valueOf(height, width, depth);

                entry.Key.changeDimensions(resizedDimensions);
            }
        }

        /// <summary>
        /// Builds a Dictionary detailing how the slots will be laid out.
        /// When adding a Slot, currentSlotWidth should be 0 and newSlotWidth should be the value of the Slot's width. The list should contain the new Slot.
        /// When resizing a Slot, currentSlotWidth should be it's current width value and newSlotWidth should be its new value. The list should be the instance's list of Slot.
        /// When removing a slot, currentSlotWidth should be it's current width value and newSlotWidth should be 0 and the list should be the instance's list of Slot.
        /// </summary>
        /// <param name="currentSlotWidth">Slot's current width.</param>
        /// <param name="newSlotWidth">Slot's new width.</param>
        /// <param name="slotBeingResized">Instance of Slot being resized.</param>
        /// <param name="slots">List containing instances of Slot.</param>
        /// <returns>A Dictionary detailing how the slots will be structured.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the Slot can't be resized.</exception>
        private IDictionary<Slot, double> buildSlotLayoutDictionary(double currentSlotWidth, double newSlotWidth, Slot slotBeingResized, List<Slot> slots) {
            Dictionary<Slot, double> slotWidthMap = new Dictionary<Slot, double>();

            //build dictionary with slots and their current widths
            foreach (Slot slot in slots) {
                if (slot.Equals(slotBeingResized)) {
                    slotWidthMap.Add(slot, newSlotWidth);
                } else {
                    slotWidthMap.Add(slot, slot.slotDimensions.width);
                }
            }

            double minPossibleWidth = this.product.slotWidths.minWidth;
            double maxPossibleWidth = this.product.slotWidths.maxWidth;

            int slotIndex = slots.LastIndexOf(slotBeingResized);
            int currentIndex = slotIndex;
            int lastIndex = slots.Count - 1;

            //value will be negative when decreasing
            //and positive when increasing
            double widthToBeResized = newSlotWidth - currentSlotWidth;

            //width that has been resized so far
            double resizedWidth = 0;

            while (true) {
                //if the end is reached, go to the beginning of the list
                if (currentIndex == lastIndex) {
                    currentIndex = 0;
                } else {
                    //otherwise, iterate normally
                    currentIndex++;
                }

                //exit if it has looped around or has finished resizing
                if (currentIndex == slotIndex || resizedWidth == Math.Abs(widthToBeResized)) {
                    break;
                }

                Slot otherSlot = slots[currentIndex];

                double currentOtherSlotWidth = slotWidthMap[otherSlot];

                double newOtherSlotWidth = currentOtherSlotWidth - (widthToBeResized - resizedWidth);

                if (newOtherSlotWidth <= minPossibleWidth) {
                    resizedWidth += (currentOtherSlotWidth - minPossibleWidth);
                    slotWidthMap[otherSlot] = minPossibleWidth;
                } else if (newOtherSlotWidth >= maxPossibleWidth) {
                    resizedWidth += (currentOtherSlotWidth - maxPossibleWidth);
                    slotWidthMap[otherSlot] = maxPossibleWidth;
                } else {
                    resizedWidth += (newOtherSlotWidth - currentOtherSlotWidth);

                    //if it's in the allowed value range, change the value
                    slotWidthMap[otherSlot] = newOtherSlotWidth;
                }
            }

            double totalWidthAfterResize = slotWidthMap.Sum(entry => entry.Value);

            double customizedProductWidth = this.customizedDimensions.width;

            //check if the widths match
            if (customizedProductWidth == totalWidthAfterResize) {
                return slotWidthMap;
            }

            //this should never happen when using this method to remove a slot
            //it should only happen when adding or resizing invalidates other slots
            throw new ArgumentException(UNABLE_TO_RESIZE_SLOT);
        }

        /// <summary>
        /// Checks if the provided CustomizedDimensions don't exceed those of the CustomizedProduct.
        /// </summary>
        /// <param name="customizedDimensions">Instance of CustomizedDimensions being checked.</param>
        /// <returns>true if none of the three dimensional components are greater than those of the CustomizedProduct; false, otherwise.</returns>
        private bool smallerThanCustomizedProduct(CustomizedDimensions customizedDimensions) {
            return this.customizedDimensions.height >= customizedDimensions.height
                && this.customizedDimensions.depth >= customizedDimensions.depth
                && this.customizedDimensions.width >= customizedDimensions.width;
        }

        /// <summary>
        /// Checks if the provided width is within the Product's specified range for slot widths.
        /// </summary>
        /// <param name="width">Double representing the width value being checked.</param>
        /// <returns>true if the provided width is within range of values; false, otherwise.</returns>
        private bool isWithinProductSlotWidthsRange(double width) {
            return width >= this.product.slotWidths.minWidth && width <= this.product.slotWidths.maxWidth;
        }

        /// <summary>
        /// Adds a CustomizedProduct to a Slot.
        /// </summary>
        /// <param name="childCustomizedProduct">CustomizedProduct being added.</param>
        /// <param name="slot">Slot in which the CustomizedProduct will be inserted.</param>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when the CustomizedProduct's CustomizationStatus is Finished or when the Product does not support slots.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Thrown when the provided CustomizedProduct or Slot are null or the provided CustomizedProduct does not reference one of the Product's components.
        /// </exception>
        public void addCustomizedProduct(CustomizedProduct childCustomizedProduct, Slot slot) {
            if (this.status == CustomizationStatus.FINISHED) throw new InvalidOperationException(ACTION_AFTER_CUSTOMIZATION_FINISHED);

            if (childCustomizedProduct == null) throw new ArgumentException(ADD_NULL_CUSTOMIZED_PRODUCT);

            if (slot == null) throw new ArgumentException(ADD_CUSTOMIZED_PRODUCT_TO_NULL_SLOT);

            if (childCustomizedProduct.customizedMaterial == null) throw new ArgumentException(ADD_NULL_CUSTOMIZED_MATERIAL);

            //search for a slot that matches the given slot
            Slot equivalentSlot = this.slots.Where(s => s.Equals(slot)).SingleOrDefault();

            if (equivalentSlot == null) throw new ArgumentException(SLOT_NOT_FOUND);

            //check if the customized product's product is a possible component
            IEnumerable<Product> availableChildProducts = this.product.components.Select(cp => cp.complementaryProduct);

            bool matchesComponent = availableChildProducts.Contains(childCustomizedProduct.product);

            if (!matchesComponent) throw new ArgumentException(CUSTOMIZED_PRODUCT_DOES_NOT_MATCH_CHILDREN);

            //check if customized product fulfills all of the restrictions of its father
            Product childProduct = childCustomizedProduct.product;
            Product restrictedProduct = this.product.applyRestrictionsToProduct(this, childProduct);
            checkIfChildFulfillsRestrictions(childCustomizedProduct, restrictedProduct);

            equivalentSlot.addCustomizedProduct(childCustomizedProduct);
            //update child's reference to the slot in which it's inserted
            childCustomizedProduct.insertedInSlot = equivalentSlot;
        }


        /// <summary>
        /// Removes a child CustomizedProduct from a given Slot.
        /// </summary>
        /// <param name="childCustomizedProduct">CustomizedProduct being removed.</param>
        /// <param name="slot">Slot where the CustomizedProduct is inserted.</param>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when the CustomizedProduct's CustomizationStatus is Finished.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Thrown when the provided CustomizedProduct or Slot are null or when the provided Slot could not be found in the CustomizedProduct's slots.
        /// </exception>
        public void removeCustomizedProduct(CustomizedProduct childCustomizedProduct, Slot slot) {
            if (this.status == CustomizationStatus.FINISHED) throw new InvalidOperationException(ACTION_AFTER_CUSTOMIZATION_FINISHED);

            if (childCustomizedProduct == null) throw new ArgumentException(ADD_NULL_CUSTOMIZED_PRODUCT);

            if (slot == null) throw new ArgumentException(REMOVE_NULL_CUSTOMIZED_PRODUCT);

            //search for a slot that matches the given slot
            Slot equivalentSlot = this.slots.Where(s => s.Equals(slot)).SingleOrDefault();

            if (equivalentSlot == null) throw new ArgumentException(SLOT_NOT_FOUND);

            equivalentSlot.removeCustomizedProduct(childCustomizedProduct);
        }


        /// <summary>
        /// Checks if the CustomizedProduct has had other instances of CustomizedProduct added to its slots.
        /// </summary>
        /// <returns>true if the CustomizedProduct has sub CustomizedProducts; false, otherwise.</returns>
        public bool hasCustomizedProducts() {
            foreach (Slot addedSlot in this.slots) {
                //check if any of the slots already have customized products
                if (addedSlot.hasCustomizedProducts()) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Finalizes the CustomizedProduct.
        /// </summary>
        public void finalizeCustomization() {
            //sub customized products can't finalize the customization process
            if (this.insertedInSlot != null) {
                throw new InvalidOperationException(CHILD_STARTING_FINALIZATION);
            }

            finalizeCustomization(this);
        }

        /// <summary>
        /// Recursively sets the customization's status for its children.
        /// </summary>
        /// <param name="currentCustomizedProduct">CustomizedProduct being finalized.</param>
        private void finalizeCustomization(CustomizedProduct currentCustomizedProduct) {
            //hashset with all the mandatory child products
            HashSet<Product> mandatoryProducts = currentCustomizedProduct.product
                .components.Where(cmp => cmp.mandatory).Select(cmp => cmp.complementaryProduct).ToHashSet();

            HashSet<Product> addedProducts = new HashSet<Product>();

            foreach (Slot slot in currentCustomizedProduct.slots) {
                foreach (CustomizedProduct childCustomizedProduct in slot.customizedProducts) {
                    addedProducts.Add(childCustomizedProduct.product);
                    finalizeCustomization(childCustomizedProduct);
                }
            }

            //if any of the customized products does not have material, then the customization process can't be concluded
            if (currentCustomizedProduct.customizedMaterial == null) throw new ArgumentException(FINALIZING_WITHOUT_MATERIAL);

            //check if all the mandatory components were added to the customization
            if (!mandatoryProducts.IsSubsetOf(addedProducts)) throw new InvalidOperationException(FINALIZING_WITHOUT_MANDATORY_COMPONENTS);

            currentCustomizedProduct.status = CustomizationStatus.FINISHED;
        }

        /// <summary>
        /// Returns the number of Slots in the CustomizedProduct's Slot list
        /// </summary>
        /// <returns>Number of Slots within the CustomizedProduct</returns>
        public int numberOfSlots() {
            return slots.Count;
        }

        /// <summary>
        /// Returns the number of sub CustomizedProducts.
        /// </summary>
        /// <returns>Number of sub CustomizedProducts within the CustomizedProduct.</returns>
        public int numberOfSubCustomizedProducts() {
            int result = 0;

            foreach (Slot slot in this.slots) {
                result += slot.customizedProducts.Count;
            }

            return result;
        }


        /// <summary>
        /// Returns the recommended slots
        /// </summary>
        /// <returns>List with the recommended slots</returns>
        public List<CustomizedDimensions> recommendedSlots() {

            List<CustomizedDimensions> recommendedSlots = new List<CustomizedDimensions>();

            var widthCloset = customizedDimensions.width;
            var depthCloset = customizedDimensions.depth;
            var heightCloset = customizedDimensions.height;
            var unitCloset = "mm"; //customizedDimensions.unit;
            var unitSlots = "mm";
            var recommendedSlotWidth = product.slotWidths.recommendedWidth;
            var minSlotWidth = product.slotWidths.minWidth;

            var reasonW = 404.5 / widthCloset;
            var reasonD = 100 / depthCloset;
            var reasonH = 300 / heightCloset;

            var recommendedNumberSlots = (int)(widthCloset / recommendedSlotWidth);
            var remainder = widthCloset % recommendedSlotWidth;
            var remainderWidth =
              widthCloset - recommendedNumberSlots * recommendedSlotWidth;
            for (var i = 0; i < recommendedNumberSlots; i++) {
                recommendedSlots.Add(
                    CustomizedDimensions.valueOf(
                        heightCloset,
                        recommendedSlotWidth,
                        depthCloset));
            }
            if (remainderWidth > 0) {
                if (remainder > minSlotWidth) {
                    recommendedSlots.Add(
                        CustomizedDimensions.valueOf(
                            heightCloset,
                            remainderWidth,
                            depthCloset));
                } else {
                    var lackToMin = minSlotWidth - remainderWidth;
                    var takeRecommended = lackToMin / recommendedNumberSlots;

                    if ((recommendedSlotWidth - takeRecommended) > minSlotWidth) {
                        recommendedSlots = new List<CustomizedDimensions>();
                        for (var i = 0; i < recommendedNumberSlots; i++) {
                            recommendedSlots.Add(
                                CustomizedDimensions.valueOf(
                                heightCloset,
                                (recommendedSlotWidth - takeRecommended),
                                depthCloset));
                        }
                        recommendedSlots.Add(
                            CustomizedDimensions.valueOf(
                            heightCloset,
                            (minSlotWidth),
                            depthCloset));
                    }
                }
            }
            return recommendedSlots;
        }
        /// <summary>
        /// Returns the min slots
        /// </summary>
        /// <returns>List with the min slots</returns>
        public List<CustomizedDimensions> minSlots() {

            List<CustomizedDimensions> minSlots = new List<CustomizedDimensions>();

            var widthCloset = customizedDimensions.width;
            var depthCloset = customizedDimensions.depth;
            var heightCloset = customizedDimensions.height;
            var unitCloset = "mm"; //customizedDimensions.unit;
            var unitSlots = "mm";
            var maxSlotWidth = product.slotWidths.maxWidth;
            var minSlotWidth = product.slotWidths.minWidth;

            var reasonW = 404.5 / widthCloset;
            var reasonD = 100 / depthCloset;
            var reasonH = 300 / heightCloset;

            var maxNumberSlots = (int)(widthCloset / maxSlotWidth);
            var remainder = widthCloset % maxSlotWidth;
            var remainderWidth =
              widthCloset - maxNumberSlots * maxSlotWidth;
            minSlots.Add(
                CustomizedDimensions.valueOf(
                    heightCloset,
                    maxSlotWidth,
                    depthCloset));

            if (remainderWidth > 0) {
                if (remainder > minSlotWidth) {
                    minSlots.Add(
                        CustomizedDimensions.valueOf(
                            heightCloset,
                            remainderWidth,
                            depthCloset));
                } else {
                    var lackToMin = minSlotWidth - remainderWidth;
                    var takeRecommended = lackToMin / maxNumberSlots;

                    if ((maxSlotWidth - takeRecommended) > minSlotWidth) {
                        minSlots = new List<CustomizedDimensions>();
                        for (var i = 0; i < maxNumberSlots; i++) {
                            minSlots.Add(
                                CustomizedDimensions.valueOf(
                                heightCloset,
                                (maxSlotWidth - takeRecommended),
                                depthCloset));
                        }
                        minSlots.Add(
                            CustomizedDimensions.valueOf(
                            heightCloset,
                            (minSlotWidth),
                            depthCloset));
                    }
                }
            }
            return minSlots;
        }

        /// <summary>
        /// Checks if the parent CustomizedProduct is valid (not null).
        /// </summary>
        /// <param name="parentCustomizedProduct">CustomizedProduct being checked.</param>
        /// <exception cref="System.ArgumentException">Thrown when the provided parent CustomizedProduct is null.</exception>
        private void checkParentCustomizedProduct(CustomizedProduct parentCustomizedProduct) {
            if (parentCustomizedProduct == null) throw new ArgumentException(ADD_CUSTOMIZED_PRODUCT_TO_NULL_PARENT);
        }

        /// <summary>
        /// Checks if the Slot in which the CustomizedProduct will be inserted is valid (not null).
        /// </summary>
        /// <param name="insertedInSlot">Slot being checked.</param>
        /// <exception cref="System.ArgumentException">Thrown when the provided Slot is null.</exception>
        private void checkInsertedInSlot(Slot insertedInSlot) {
            if (insertedInSlot == null) throw new ArgumentException(ADD_CUSTOMIZED_PRODUCT_TO_NULL_SLOT);
        }

        /// <summary>
        /// Checks if the Product is valid (not null).
        /// </summary>
        /// <param name="product">Product to check</param>
        /// <exception cref="System.ArgumentException">Thrown when the provided Product is null.</exception>


        private void checkProduct(Product product) {

            if (product == null) throw new ArgumentException(INVALID_PRODUCT);
        }

        /// <summary>
        /// Checks if the CustomizedMaterial is valid
        /// </summary>
        /// <param name="customizedMaterial">CustomizedMaterial to check</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when the provided instance of CustomizedMaterial is null or its Material is not one of the Product's materials.
        /// </exception>
        private void checkCustomizedMaterial(CustomizedMaterial customizedMaterial) {
            if (customizedMaterial == null) throw new ArgumentException(INVALID_CUSTOMIZED_PRODUCT_MATERIAL);
            if (!this.product.containsMaterial(customizedMaterial.material)) throw new ArgumentException(INVALID_CUSTOMIZED_PRODUCT_MATERIAL);
        }

        /// <summary>
        /// Checks if the CustomizedDimensions are valid, that means that they are not null and 
        /// that they must represent a selection of values available in the Product's collection of Measurement.
        /// </summary>
        /// <param name="customizedDimensions">CustomizedDimensions to check</param>
        /// <param name="product">Product to which this instance of CustomizedProduct is associated.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when the provided instance of CustomizedDimensions is null or does not represent values available in the Product's collection of Measurement.
        /// </exception>
        private void checkCustomizedDimensions(CustomizedDimensions customizedDimensions, Product product) {
            if (customizedDimensions == null) throw new ArgumentException(INVALID_CUSTOMIZED_PRODUCT_DIMENSIONS);

            List<Measurement> possibleMeasurements = product.productMeasurements.Select(m => m.measurement).ToList();

            foreach (Measurement measurement in possibleMeasurements) {
                double height = customizedDimensions.height;
                double width = customizedDimensions.width;
                double depth = customizedDimensions.depth;

                bool hasDimensionValues = measurement.hasValues(height, width, depth);

                if (hasDimensionValues) {
                    return; //return immediately if all the values match
                }
            }

            throw new ArgumentException(CUSTOMIZED_PRODUCT_DIMENSIONS_NOT_MATCHING_SPECIFICATION);
        }

        /// <summary>
        /// Checks if a given string is valid (not null nor empty).
        /// </summary>
        /// <param name="obj">String to check</param>
        /// <exception cref="System.ArgumentException">Thrown when the provided string is null or empty.</exception>
        private void checkString(string obj, string message) {
            if (String.IsNullOrEmpty(obj)) throw new ArgumentException(message);
        }

        /// <summary>
        /// Checks if a CustomizedProduct fulfills all restrictions
        /// </summary>
        /// <param name="childCustomizedProduct"></param>
        /// <param name="restrictedChild"></param>
        private void checkIfChildFulfillsRestrictions(CustomizedProduct childCustomizedProduct, Product restrictedChild) {
            bool containsMaterial = false;
            foreach (ProductMaterial pmat in restrictedChild.productMaterials) {
                if (pmat.material.Equals(childCustomizedProduct.customizedMaterial.material)) {
                    containsMaterial = true;
                    Material mat = pmat.material;
                    if (!mat.Finishes.Contains(childCustomizedProduct.customizedMaterial.finish)) {
                        throw new ArgumentException(CHILD_FINISH_INAVLID);
                    }
                    if (!mat.Colors.Contains(childCustomizedProduct.customizedMaterial.color)) {
                        throw new ArgumentException(CHILD_COLOR_INVALID);
                    }
                }
            }
            if (!containsMaterial) {
                throw new ArgumentException(CHILD_MATERIAL_INVALID);
            }
            bool containsDimension = false;
            foreach (ProductMeasurement pmeas in restrictedChild.productMeasurements) {
                Measurement measurement = pmeas.measurement;
                double height = childCustomizedProduct.customizedDimensions.height;
                double width = childCustomizedProduct.customizedDimensions.width;
                double depth = childCustomizedProduct.customizedDimensions.depth;
                if (measurement.hasValues(height, width, depth)) {
                    containsDimension = true;
                    break;
                }
            }
            if (!containsDimension) {
                throw new ArgumentException(CHILD_DIMENSION_INVALID);
            }
        }

        /// <summary>
        /// Returns the CustomizedProduct's identity
        /// </summary>
        /// <returns>String with the CustomizedProduct's identity</returns>
        public string id() {
            return reference;
        }

        /// <summary>
        /// Checks if the CustomizedProduct's identity is the same as the one received as a parameter
        /// </summary>
        /// <param name="comparingEntity">Entity to compare to the CustomizedProduct</param>
        /// <returns>true if the given identifier is equal to the CustomizedProduct's identity; false, otherwise.</returns>
        public bool sameAs(string comparingEntity) {
            return reference.Equals(comparingEntity, StringComparison.InvariantCultureIgnoreCase);
        }


        public override bool activate() {
            if (activated) {
                return false;
            }

            activated = true;

            foreach (Slot slot in this.slots) {
                foreach (CustomizedProduct customizedProduct in slot.customizedProducts) {
                    return customizedProduct.activate();
                }
            }

            return true;
        }

        public override bool deactivate() {
            if (!activated) {
                return false;
            }

            activated = false;

            foreach (Slot slot in this.slots) {
                foreach (CustomizedProduct customizedProduct in slot.customizedProducts) {
                    return customizedProduct.deactivate();
                }
            }

            return true;
        }

        /// <summary>
        /// Returns a textual description of the CustomizedProduct
        /// </summary>
        /// <returns>String that describes the CustomizedProduct</returns>
        public override string ToString() {
            return string.Format("Designation: {0}, Reference {1}", designation, reference);
        }

        /// <summary>
        /// Returns the generated hash code of the CustomizedProduct
        /// </summary>
        /// <returns>Generated hash code</returns>
        public override int GetHashCode() {
            int hashCode = 17;
            hashCode = hashCode * 23 + this.reference.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Checks if a certain CustomizedProduct is the same as a received object
        /// </summary>
        /// <param name="obj">Object to compare with the CustomizedProduct</param>
        /// <returns>true if both objects are equal, false if not</returns>
        public override bool Equals(object obj) {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType())) {
                return false;
            } else {
                CustomizedProduct other = (CustomizedProduct)obj;
                return this.reference.Equals(other.reference);
            }
        }

        /// <summary>
        /// Returns the current CustomizedProduct as a DTO
        /// </summary>
        /// <returns>CustomizedProductDTO with the current representation of the CustomizedProduct</returns>
        public CustomizedProductDTO toDTO() {
            CustomizedProductDTO dto = new CustomizedProductDTO();
            dto.reference = this.reference;
            dto.designation = this.designation;
            dto.productDTO = this.product.toDTO();
            dto.customizedDimensionsDTO = this.customizedDimensions.toDTO();
            dto.customizedMaterialDTO = this.customizedMaterial.toDTO();
            dto.slotListDTO = DTOUtils.parseToDTOS(this.slots).ToList();
            dto.id = this.Id;
            return dto;
        }


        /// <summary>
        /// Enumerate of values describing the CustomizedProduct's customization status.
        /// </summary>
        public enum CustomizationStatus {
            PENDING,
            FINISHED
        }

        /// <summary>
        /// CustomizedProduct's Builder.
        /// </summary>
        public class CustomizedProductBuilder {
            /// <summary>
            /// Instance of CustomizedProduct being built.
            /// </summary>
            private CustomizedProduct customizedProduct;

            /// <summary>
            /// Builder's private empty constructor, used for hiding the implicit public one.
            /// </summary>
            private CustomizedProductBuilder() { }

            /// <summary>
            /// Creates an instance of CustomizedProductBuilder, responsible for building an instance of CustomizedProduct.
            /// </summary>
            /// <param name="reference">Reference assigned to the CustomizedProduct.</param>
            /// <param name="product">Product defining the specification for the CustomizedProduct.</param>
            /// <param name="customizedDimensions">Instance of CustomizedDimensions detailing the CustomizedProduct's dimensions.</param>
            /// <returns>An instance of CustomizedProductBuilder.</returns>
            public static CustomizedProductBuilder createCustomizedProduct(string reference, Product product, CustomizedDimensions customizedDimensions) {
                CustomizedProductBuilder builder = new CustomizedProductBuilder();
                builder.customizedProduct = new CustomizedProduct(reference, product, customizedDimensions);
                return builder;
            }

            /// <summary>
            /// Creates an instance of CustomizedProductBuilder, responsible for building an instance of CustomizedProduct.
            /// </summary>
            /// <param name="authToken">Authentication token of the user creating the CustomizedProduct.</param>
            /// <param name="reference">Reference assigned to the CustomizedProduct.</param>
            /// <param name="product">Product defining the specification for the CustomizedProduct.</param>
            /// <param name="customizedDimensions">Instance of CustomizedDimensions detailing the CustomizedProduct's dimensions.</param>
            /// <returns>An instance of CustomizedProductBuilder.</returns>
            public static CustomizedProductBuilder createCustomizedProduct(string authToken, string reference, Product product, CustomizedDimensions customizedDimensions) {
                CustomizedProductBuilder builder = new CustomizedProductBuilder();
                builder.customizedProduct = new CustomizedProduct(authToken, reference, product, customizedDimensions);
                return builder;
            }

            /// <summary>
            /// Creates an instance of CustomizedProductBuilder, responsible for building an instance of CustomizedProduct.
            /// </summary>
            /// <param name="product">Product defining the specification for the CustomizedProduct.</param>
            /// <param name="customizedDimensions">Instance of CustomizedDimensions detailing the CustomizedProduct's dimensions.</param>
            /// <param name="parentCustomizedProduct">Instance of CustomizedProduct representing the parent.</param>
            /// <param name="insertedInSlot">Parent CustomizedProduct's slot in which the new CustomizedProduct will be inserted in.</param>
            /// <returns>An instance of CustomizedProductBuilder.</returns>
            public static CustomizedProductBuilder createCustomizedProduct(Product product, CustomizedDimensions customizedDimensions, CustomizedProduct parentCustomizedProduct, Slot insertedInSlot) {
                CustomizedProductBuilder builder = new CustomizedProductBuilder();
                builder.customizedProduct = new CustomizedProduct(product, customizedDimensions, parentCustomizedProduct, insertedInSlot);

                return builder;
            }

            /// <summary>
            /// Creates an instance of CustomizedProductBuilder, responsible for building an instance of CustomizedProduct.
            /// </summary>
            /// <param name="product">Product defining the specification for the CustomizedProduct.</param>
            /// <param name="authToken">Authentication token of the user creating the CustomizedProduct.</param>
            /// <param name="customizedDimensions">Instance of CustomizedDimensions detailing the CustomizedProduct's dimensions.</param>
            /// <param name="parentCustomizedProduct">Instance of CustomizedProduct representing the parent.</param>
            /// <param name="insertedInSlot">Parent CustomizedProduct's slot in which the new CustomizedProduct will be inserted in.</param>
            /// <returns>An instance of CustomizedProductBuilder.</returns>
            public static CustomizedProductBuilder createCustomizedProduct(string authToken, Product product, CustomizedDimensions customizedDimensions, CustomizedProduct parentCustomizedProduct, Slot insertedInSlot) {
                CustomizedProductBuilder builder = new CustomizedProductBuilder();
                builder.customizedProduct = new CustomizedProduct(authToken, product, customizedDimensions, parentCustomizedProduct, insertedInSlot);

                return builder;
            }

            /// <summary>
            /// Adds a designation to the CustomizedProduct being built.
            /// </summary>
            /// <param name="designation">The CustomizedProduct's designation.</param>
            /// <returns>An instance of CustomizedProductBuilder.</returns>
            public CustomizedProductBuilder withDesignation(string designation) {
                this.customizedProduct.changeDesignation(designation);
                return this;
            }

            /// <summary>
            /// Adds an instance of CustomizedMaterial to the CustomizedProduct being built.
            /// </summary>
            /// <param name="customizedMaterial">Instance of CustomizedMaterial that will be assigned to the built CustomizedProduct.</param>
            /// <returns>An instance of CustomizedProductBuilder.</returns>
            public CustomizedProductBuilder withMaterial(CustomizedMaterial customizedMaterial) {
                this.customizedProduct.changeCustomizedMaterial(customizedMaterial);
                return this;
            }

            /// <summary>
            /// Builds the instance of CustomizedProduct.
            /// </summary>
            /// <returns>Built instance of CustomizedProduct</returns>
            public CustomizedProduct build() {
                return this.customizedProduct;
            }
        }


        /// <summary>
        /// Builds an identifier for a CustomizedProduct's slot. 
        /// e.g.: CPIdentifier-S4, which means it's the 4th slot in that CustomizedProduct.
        /// </summary>
        /// <param name="customizedProduct">CustomizedProduct.</param>
        private static string buildSlotIdentifier(CustomizedProduct customizedProduct) {
            int number = customizedProduct.numberOfSlots() + 1;

            return string.Concat(customizedProduct.reference, SLOT_IDENTIFIER_DELIMITER, number);
        }

        /// <summary>
        /// Builds a reference for a sub CustomizedProduct. 
        /// e.g.: ParentReference-CP5, which means it's the 5th direct child of that parent.
        /// </summary>
        /// <param name="parentCustomizedProduct">Parent CustomizedProduct, on which the reference will be based on.</param>
        /// <returns>string representing the sub CustomizedProduct's reference.</returns>
        private static string buildSubCustomizedProductReference(CustomizedProduct parentCustomizedProduct) {
            int number = parentCustomizedProduct.numberOfSubCustomizedProducts() + 1;

            return string.Concat(parentCustomizedProduct.reference, REFERENCE_DELIMITER, number);
        }
    }
}