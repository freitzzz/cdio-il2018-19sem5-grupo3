using System.Collections.Generic;
using support.domain.ddd;
using System;
using support.dto;
using core.dto;
using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using support.domain;

namespace core.domain
{
    /// <summary>
    /// Class that represents a Customized Product.
    /// </summary>
    public class CustomizedProduct : Activatable, AggregateRoot<string>, DTOAble<CustomizedProductDTO>
    {

        /// <summary>
        /// Constant that represents the message that occurs if the CustomizedProduct's material is not valid
        /// </summary>
        private const string INVALID_CUSTOMIZED_PRODUCT_MATERIAL = "The chosen material is not valid";

        /// <summary>
        /// Constant that represents the message that occurs if the CustomizedProduct's dimensions are not valid
        /// </summary>
        private const string INVALID_CUSTOMIZED_PRODUCT_DIMENSIONS = "The inserted dimensions is not valid";

        /// <summary>
        /// Constant that represents the message presented if provided CustomizedProduct's dimensions do not match any of Product's Measurements.
        /// </summary>
        private const string CUSTOMIZED_PRODUCT_DIMENSIONS_NOT_MATCHING_SPECIFICATION = "The inserted dimensions don't match the product's specification.";

        /// <summary>
        /// Constant that represents the message that occurs if the CustomizedProduct's product reference is not valid
        /// </summary>
        private const string INVALID_PRODUCT_REFERENCE = "The inserted product reference is not valid";

        /// <summary>
        /// Constant that represents the message presented if the CustomizedProduct's user authentication token is not valid.
        /// </summary>
        private const string INVALID_AUTH_TOKEN = "The inserted authentication token is not valid.";

        /// <summary>
        /// Constant that represents the message that occurs if the CustomizedProduct's product is not valid
        /// </summary>
        private const string INVALID_PRODUCT = "The inserted product is not valid.";

        /// <summary>
        /// Constant that represents the message that occurs if the CustomizedProduct's designation is not valid
        /// </summary>
        private const string INVALID_PRODUCT_DESIGNATION = "The inserted designation is not valid";

        /// <summary>
        /// Constant that represents the message that occurs if the CustomizedProduct's slot is not valid
        /// </summary>
        private const string INVALID_INSERTED_IN_SLOT = "The customized products own slot is not valid";

        /// <summary>
        /// Constant that represents the message that occurs if the CustomizedProduct's slots is null
        /// </summary>
        private const string NULL_SLOT = "The customized products slots cannot be null";

        /// <summary>
        /// Constant that represents the message that if a Slot with null CustomizedDimensions is attempted to be added.
        /// </summary>
        private const string NULL_SLOT_DIMENSIONS = "Unable to add a slot with null dimensions.";

        /// <summary>
        /// Constant that represents the message that occurs if the CustomizedProduct's product doesn't support slots
        /// </summary>
        private const string PRODUCT_DOES_NOT_SUPPORT_SLOTS = "This customized product doesn't support slots";

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
        private const string ADD_NULL_CUSTOMIZED_PRODUCT = "Unable add a null customized product.";

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
        private CustomizedProduct(ILazyLoader lazyLoader)
        {
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
        public CustomizedProduct(string reference, Product product, CustomizedDimensions customizedDimensions)
        {
            checkString(reference, INVALID_PRODUCT_REFERENCE);
            checkProduct(product);
            checkCustomizedDimensions(customizedDimensions, product);
            this.reference = reference;
            this.product = product;
            this.customizedDimensions = customizedDimensions;
            this.status = CustomizationStatus.PENDING;
            this.slots = new List<Slot>();
            //Add slot matching the CustomizedProduct's dimensions
            this.slots.Add(new Slot(id() + this.slots.Count, customizedDimensions));
        }

        /// <summary>
        /// Creates a new instance of CustomizedProduct with a given reference, based on the specifications of a given Product,
        /// with the given authentication token.
        /// </summary>
        /// <param name="authToken">Authentication token of the user creating this CustomizedProduct.</param>
        /// <param name="reference">Reference assigned to this CustomizedProduct.</param>
        /// <param name="product">Product defining the specification for this CustomizedProduct.</param>
        /// <param name="customizedDimensions">Instance of CustomizedDimensions detailing the CustomizedProduct's dimensions.</param>
        public CustomizedProduct(string authToken, string reference, Product product, CustomizedDimensions customizedDimensions)
            : this(reference, product, customizedDimensions)
        {
            checkString(authToken, INVALID_AUTH_TOKEN);
            this.authToken = authToken;
        }

        /// <summary>
        /// Changes the CustomizedProduct's reference.
        /// </summary>
        /// <param name="reference">New reference.</param>
        public void changeReference(string reference)
        {
            if (this.status == CustomizationStatus.FINISHED) throw new InvalidOperationException(ACTION_AFTER_CUSTOMIZATION_FINISHED);
            if (String.IsNullOrEmpty(reference)) throw new ArgumentException(INVALID_PRODUCT_REFERENCE);
            this.reference = reference;
        }

        /// <summary>
        /// Changes the CustomizedProduct's designation.
        /// </summary>
        /// <param name="designation">New designation.</param>
        public void changeDesignation(string designation)
        {
            if (this.status == CustomizationStatus.FINISHED) throw new InvalidOperationException(ACTION_AFTER_CUSTOMIZATION_FINISHED);
            if (String.IsNullOrEmpty(designation)) throw new ArgumentException(INVALID_PRODUCT_DESIGNATION);
            this.designation = designation;
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
        public void changeDimensions(CustomizedDimensions customizedDimensions)
        {
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
        public void changeCustomizedMaterial(CustomizedMaterial customizedMaterial)
        {
            if (this.status == CustomizationStatus.FINISHED) throw new InvalidOperationException(ACTION_AFTER_CUSTOMIZATION_FINISHED);
            checkCustomizedMaterial(customizedMaterial);
            this.customizedMaterial = customizedMaterial;
        }

        /// <summary>
        /// Changes the Finish of a CustomizedProduct's customized material
        /// </summary>
        /// <param name="finish">new finish</param>
        public void changeFinish(Finish finish)
        {
            if (this.status == CustomizationStatus.FINISHED) throw new InvalidOperationException(ACTION_AFTER_CUSTOMIZATION_FINISHED);
            if (this.customizedMaterial == null) throw new InvalidOperationException(CHANGE_FINISH_BEFORE_DEFINING_MATERIAL);
            //TODO: inform why it was not changed
            this.customizedMaterial.changeFinish(finish);
        }

        /// <summary>
        /// Changes the Color of a CustomizedProduct's customized material
        /// </summary>
        /// <param name="color">new color</param>
        public void changeColor(Color color)
        {
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
        public void addSlot(CustomizedDimensions slotDimensions)
        {
            if (this.status == CustomizationStatus.FINISHED) throw new InvalidOperationException(ACTION_AFTER_CUSTOMIZATION_FINISHED);

            if (!this.product.supportsSlots) throw new InvalidOperationException(PRODUCT_DOES_NOT_SUPPORT_SLOTS);

            if (slotDimensions == null) throw new ArgumentException(NULL_SLOT_DIMENSIONS);

            if (!isWithinProductSlotWidthsRange(slotDimensions.width)) throw new ArgumentException(SLOT_DIMENSIONS_NOT_RESPECTING_SPECIFICATION);

            if (!smallerThanCustomizedProduct(slotDimensions)) throw new ArgumentException(SLOT_LARGER_THAN_CUSTOMIZED_PRODUCT);

            if (hasCustomizedProducts()) throw new InvalidOperationException(ADD_SLOT_AFTER_ADDING_CUSTOMIZED_PRODUCTS);

            //check if there's only the slot matching the CustomizedProduct's dimensions
            if (this.slots.Count == 1)
            {
                //the initial slot must be resized
                Slot fullSizeSlot = this.slots.FirstOrDefault();

                double updatedWidth = fullSizeSlot.slotDimensions.width - slotDimensions.width;

                //if minimum width can't be respected throw exception
                if (!isWithinProductSlotWidthsRange(updatedWidth)) throw new ArgumentException(SLOT_DIMENSIONS_NOT_RESPECTING_SPECIFICATION);

                fullSizeSlot.changeDimensions(CustomizedDimensions.valueOf(customizedDimensions.height, updatedWidth, customizedDimensions.depth));

                slots.Add(new Slot(id() + slots.Count, slotDimensions));
            }
            else
            {
                addSubsequentSlot(slotDimensions);
            }
        }

        /// <summary>
        /// Adds a new Slot after having added other Slots.
        /// </summary>
        /// <param name="slotDimensions">New Slot's CustomizedDimensions.</param>
        private void addSubsequentSlot(CustomizedDimensions slotDimensions)
        {
            double minPossibleWidth = this.product.slotWidths.minWidth;

            double idealSlotWidth = slotDimensions.width / (double)numberOfSlots();

            double availableWidth = 0;

            /*dictionary in which keys are slots and values are arrays with 2 positions 
            in which the first position is the slot's width and the second position is the value that can be expended*/
            Dictionary<Slot, double[]> slotDictionary = new Dictionary<Slot, double[]>();

            //build dictionary
            foreach (Slot slot in this.slots)
            {
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

            foreach (Slot slot in this.slots)
            {
                double oldHeight = slot.slotDimensions.height;
                double updatedWidth = slotDictionary[slot][0];
                double oldDepth = slot.slotDimensions.depth;

                CustomizedDimensions newDimensions = CustomizedDimensions.valueOf(oldHeight, updatedWidth, oldDepth);

                slot.changeDimensions(newDimensions);
            }

            Slot newSlot = new Slot(id() + this.slots.Count, slotDimensions);

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
            Dictionary<Slot, double[]> slotDictionary)
        {
            double currentSlotWidth = slotDictionary[largestSlot][0];

            double widthThatSlotCanExpended = idealSlotWidth;

            if (currentSlotWidth - idealSlotWidth < minPossibleWidth)
            {
                widthThatSlotCanExpended = currentSlotWidth - minPossibleWidth;
            }

            double slotWidthAfterSubtractingIdealSlotWidth = currentSlotWidth - widthThatSlotCanExpended;

            slotDictionary[largestSlot][0] = slotWidthAfterSubtractingIdealSlotWidth;
            slotDictionary[largestSlot][1] = slotWidthAfterSubtractingIdealSlotWidth - minPossibleWidth;

            availableWidth += widthThatSlotCanExpended;

            if (availableWidth == newSlotDimensions.width)
            {
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
        public void removeSlot(Slot slot)
        {
            if (this.status == CustomizationStatus.FINISHED) throw new InvalidOperationException(ACTION_AFTER_CUSTOMIZATION_FINISHED);

            if (slot == null) throw new ArgumentException(NULL_SLOT);

            Slot slotBeingRemoved = this.slots.Where(s => s.Equals(slot)).SingleOrDefault();

            if (slotBeingRemoved == null) throw new ArgumentException(SLOT_NOT_FOUND);

            if (this.slots.Count == 1) throw new InvalidOperationException(REMOVE_LAST_SLOT);

            if (hasCustomizedProducts()) throw new InvalidOperationException(ADD_SLOT_AFTER_ADDING_CUSTOMIZED_PRODUCTS);

            if (this.slots.Count == 2)
            {
                //the last slot has to match the product's dimensions
                this.slots.Remove(slotBeingRemoved);

                Slot mainSlot = this.slots.SingleOrDefault();

                mainSlot.changeDimensions(this.customizedDimensions);
            }
            else
            {
                double previousSlotWidth = slotBeingRemoved.slotDimensions.width;

                IDictionary<Slot, double> slotWidthMap = buildSlotLayoutDictionary(previousSlotWidth, 0, slotBeingRemoved, this.slots);

                foreach (KeyValuePair<Slot, double> entry in slotWidthMap)
                {
                    //remove the desired slot and resize all the others
                    if (entry.Key.Equals(slotBeingRemoved))
                    {
                        this.slots.Remove(slotBeingRemoved);
                    }
                    else
                    {
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
        public void resizeSlot(Slot slot, CustomizedDimensions newSlotDimensions)
        {
            if (this.status == CustomizationStatus.FINISHED) throw new InvalidOperationException(ACTION_AFTER_CUSTOMIZATION_FINISHED);

            if (slot == null) throw new ArgumentException(NULL_SLOT);

            if (!isWithinProductSlotWidthsRange(newSlotDimensions.width)) throw new ArgumentException(SLOT_DIMENSIONS_NOT_RESPECTING_SPECIFICATION);

            Slot slotBeingResized = this.slots.Where(s => s.Equals(slot)).SingleOrDefault();

            if (slotBeingResized == null) throw new ArgumentException(SLOT_NOT_FOUND);

            if (this.slots.Count == 1) throw new InvalidOperationException(RESIZE_ONLY_SLOT);

            //skip doing all the other operations
            if (slotBeingResized.slotDimensions.Equals(newSlotDimensions)) return;

            IDictionary<Slot, double> slotWidthMap = buildSlotLayoutDictionary(slotBeingResized.slotDimensions.width, newSlotDimensions.width, slotBeingResized, this.slots);

            //resize all the slots with the values in the Dictionary
            foreach (KeyValuePair<Slot, double> entry in slotWidthMap)
            {
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
        private IDictionary<Slot, double> buildSlotLayoutDictionary(double currentSlotWidth, double newSlotWidth, Slot slotBeingResized, List<Slot> slots)
        {
            Dictionary<Slot, double> slotWidthMap = new Dictionary<Slot, double>();

            //build dictionary with slots and their current widths
            foreach (Slot slot in slots)
            {
                if (slot.Equals(slotBeingResized))
                {
                    slotWidthMap.Add(slot, newSlotWidth);
                }
                else
                {
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

            while (true)
            {
                //if the end is reached, go to the beginning of the list
                if (currentIndex == lastIndex)
                {
                    currentIndex = 0;
                }
                else
                {
                    //otherwise, iterate normally
                    currentIndex++;
                }

                //exit if it has looped around or has finished resizing
                if (currentIndex == slotIndex || resizedWidth == Math.Abs(widthToBeResized))
                {
                    break;
                }

                Slot otherSlot = slots[currentIndex];

                double currentOtherSlotWidth = slotWidthMap[otherSlot];

                double newOtherSlotWidth = currentOtherSlotWidth - (widthToBeResized - resizedWidth);

                if (newOtherSlotWidth <= minPossibleWidth)
                {
                    resizedWidth += (currentOtherSlotWidth - minPossibleWidth);
                    slotWidthMap[otherSlot] = minPossibleWidth;
                }
                else if (newOtherSlotWidth >= maxPossibleWidth)
                {
                    resizedWidth += (currentOtherSlotWidth - maxPossibleWidth);
                    slotWidthMap[otherSlot] = maxPossibleWidth;
                }
                else
                {
                    resizedWidth += (newOtherSlotWidth - currentOtherSlotWidth);

                    //if it's in the allowed value range, change the value
                    slotWidthMap[otherSlot] = newOtherSlotWidth;
                }
            }

            double totalWidthAfterResize = slotWidthMap.Sum(entry => entry.Value);

            double customizedProductWidth = this.customizedDimensions.width;

            //check if the widths match
            if (customizedProductWidth == totalWidthAfterResize)
            {
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
        private bool smallerThanCustomizedProduct(CustomizedDimensions customizedDimensions)
        {
            return this.customizedDimensions.height >= customizedDimensions.height
                && this.customizedDimensions.depth >= customizedDimensions.depth
                && this.customizedDimensions.width >= customizedDimensions.width;
        }

        /// <summary>
        /// Checks if the provided width is within the Product's specified range for slot widths.
        /// </summary>
        /// <param name="width">Double representing the width value being checked.</param>
        /// <returns>true if the provided width is within range of values; false, otherwise.</returns>
        private bool isWithinProductSlotWidthsRange(double width)
        {
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
        public void addCustomizedProduct(CustomizedProduct childCustomizedProduct, Slot slot)
        {
            if (this.status == CustomizationStatus.FINISHED) throw new InvalidOperationException(ACTION_AFTER_CUSTOMIZATION_FINISHED);

            if (childCustomizedProduct == null) throw new ArgumentException(ADD_NULL_CUSTOMIZED_PRODUCT);

            if (slot == null) throw new ArgumentException(NULL_SLOT);

            //search for a slot that matches the given slot
            Slot equivalentSlot = this.slots.Where(s => s.Equals(slot)).SingleOrDefault();

            if (equivalentSlot == null) throw new ArgumentException(SLOT_NOT_FOUND);

            //check if the customized product's product is a possible component
            IEnumerable<Product> availableChildProducts = this.product.components.Select(cp => cp.complementaryProduct);

            bool matchesComponent = availableChildProducts.Contains(childCustomizedProduct.product);

            if (!matchesComponent) throw new ArgumentException(CUSTOMIZED_PRODUCT_DOES_NOT_MATCH_CHILDREN);

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
        public void removeCustomizedProduct(CustomizedProduct childCustomizedProduct, Slot slot)
        {
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
        public bool hasCustomizedProducts()
        {
            foreach (Slot addedSlot in this.slots)
            {
                //check if any of the slots already have customized products
                if (addedSlot.hasCustomizedProducts())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Finalizes the CustomizedProduct.
        /// </summary>
        public void finalizeCustomization()
        {
            //sub customized products can't finalize the customization process
            if (this.insertedInSlot != null)
            {
                throw new InvalidOperationException(CHILD_STARTING_FINALIZATION);
            }

            finalizeCustomization(this);
        }

        /// <summary>
        /// Recursively sets the customization's status for its children.
        /// </summary>
        /// <param name="currentCustomizedProduct">CustomizedProduct being finalized.</param>
        private void finalizeCustomization(CustomizedProduct currentCustomizedProduct)
        {
            //hashset with all the mandatory child products
            HashSet<Product> mandatoryProducts = currentCustomizedProduct.product
                .components.Where(cmp => cmp.mandatory).Select(cmp => cmp.complementaryProduct).ToHashSet();

            HashSet<Product> addedProducts = new HashSet<Product>();

            foreach (Slot slot in currentCustomizedProduct.slots)
            {
                foreach (CustomizedProduct childCustomizedProduct in slot.customizedProducts)
                {
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
        public int numberOfSlots()
        {
            return slots.Count;
        }
        /// <summary>
        /// Returns the recommended slots
        /// </summary>
        /// <returns>List with the recommended slots</returns>
        public List<CustomizedDimensions> recommendedSlots()
        {

            List<CustomizedDimensions> recommendedSlots = new List<CustomizedDimensions>();

            var widthCloset = //customizedDimensions.width; 
            6000;
            /*store.state.customizedProduct.customizedDimensions.width;*/ ///404.5;
            var depthCloset = //customizedDimensions.depth; 
            2500;/*store.state.customizedProduct.customizedDimensions.depth;*/ ///100;
            var heightCloset = //customizedDimensions.height; 
            5000; /*store.state.customizedProduct.customizedDimensions.height;*/ ///300;   
            var unitCloset = "mm";//customizedDimensions.unit;
                                  //store.state.customizedProduct.customizedDimensions.unit;
            var unitSlots = "mm";
            //store.getters.productSlotWidths.unit;
            var recommendedSlotWidth = product.slotWidths.recommendedWidth;
            ///store.getters.recommendedSlotWidth;
            var minSlotWidth = product.slotWidths.minWidth;
            ///store.getters.minSlotWidth;

            /* if(unitCloset != unitSlots){
              this.convert(unitSlots,unitCloset,recommendedSlotWidth);
              recommendedSlotWidth = this.valueConvertedSlotsWidth;
              this.convert(unitSlots,unitCloset,minSlotWidth);
              minSlotWidth = this.valueConvertedSlotsWidth;
            }  */

            var reasonW = 404.5 / widthCloset;
            var reasonD = 100 / depthCloset;
            var reasonH = 300 / heightCloset;

            var recommendedNumberSlots = (int)(widthCloset / recommendedSlotWidth);
            var remainder = widthCloset % recommendedSlotWidth;
            var remainderWidth =
              widthCloset - recommendedNumberSlots * recommendedSlotWidth;
            for (var i = 0; i < recommendedNumberSlots; i++)
            {
                recommendedSlots.Add(
                    CustomizedDimensions.valueOf(
                        heightCloset,
                        recommendedSlotWidth,
                        depthCloset));
            }
            if (remainderWidth > 0)
            {
                if (remainder > minSlotWidth)
                {
                    recommendedSlots.Add(
                        CustomizedDimensions.valueOf(
                            heightCloset,
                            remainderWidth,
                            depthCloset));
                }
                else
                {
                    var lackToMin = minSlotWidth - remainderWidth;
                    var takeRecommended = lackToMin / recommendedNumberSlots;

                    if ((recommendedSlotWidth - takeRecommended) > minSlotWidth)
                    {
                        recommendedSlots = new List<CustomizedDimensions>();
                        for (var i = 0; i < recommendedNumberSlots; i++)
                        {
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
        public List<CustomizedDimensions> minSlots()
        {

            List<CustomizedDimensions> minSlots = new List<CustomizedDimensions>();

            var widthCloset = //customizedDimensions.width; 
            6000;
            /*store.state.customizedProduct.customizedDimensions.width;*/ ///404.5;
            var depthCloset = //customizedDimensions.depth; 
            2500;/*store.state.customizedProduct.customizedDimensions.depth;*/ ///100;
            var heightCloset = //customizedDimensions.height; 
            5000; /*store.state.customizedProduct.customizedDimensions.height;*/ ///300;   
            var unitCloset = "mm";//customizedDimensions.unit;
                                  //store.state.customizedProduct.customizedDimensions.unit;
            var unitSlots = "mm";
            //store.getters.productSlotWidths.unit;
            var maxSlotWidth = product.slotWidths.maxWidth;
            ///store.getters.recommendedSlotWidth;
            var minSlotWidth = product.slotWidths.minWidth;
            ///store.getters.minSlotWidth;

            /* if(unitCloset != unitSlots){
              this.convert(unitSlots,unitCloset,recommendedSlotWidth);
              recommendedSlotWidth = this.valueConvertedSlotsWidth;
              this.convert(unitSlots,unitCloset,minSlotWidth);
              minSlotWidth = this.valueConvertedSlotsWidth;
            }  */

            var reasonW = 404.5 / widthCloset;
            var reasonD = 100 / depthCloset;
            var reasonH = 300 / heightCloset;

            var maxNumberSlots = (int)(widthCloset / maxSlotWidth);
            var remainder = widthCloset % maxSlotWidth;
            var remainderWidth =
              widthCloset - maxNumberSlots * maxSlotWidth;
            for (var i = 0; i < maxNumberSlots; i++)
            {
                minSlots.Add(
                    CustomizedDimensions.valueOf(
                        heightCloset,
                        maxSlotWidth,
                        depthCloset));
            }
            if (remainderWidth > 0)
            {
                if (remainder > minSlotWidth)
                {
                    minSlots.Add(
                        CustomizedDimensions.valueOf(
                            heightCloset,
                            remainderWidth,
                            depthCloset));
                }
                else
                {
                    var lackToMin = minSlotWidth - remainderWidth;
                    var takeRecommended = lackToMin / maxNumberSlots;

                    if ((maxSlotWidth - takeRecommended) > minSlotWidth)
                    {
                        minSlots = new List<CustomizedDimensions>();
                        for (var i = 0; i < maxNumberSlots; i++)
                        {
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
        /// Checks if the Product is valid (not null)
        /// </summary>
        /// <param name="product">Product to check</param>
        private void checkProduct(Product product)
        {
            if (product == null) throw new ArgumentException(INVALID_PRODUCT);
        }

        /// <summary>
        /// Checks if the CustomizedMaterial is valid
        /// </summary>
        /// <param name="customizedMaterial">CustomizedMaterial to check</param>
        private void checkCustomizedMaterial(CustomizedMaterial customizedMaterial)
        {
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
        /// Thrown when the provided instance of CustomizedDimensions do not represent values available in the Product's collection of Measurement.
        /// </exception>
        private void checkCustomizedDimensions(CustomizedDimensions customizedDimensions, Product product)
        {
            if (customizedDimensions == null) throw new ArgumentException(INVALID_CUSTOMIZED_PRODUCT_DIMENSIONS);

            List<Measurement> possibleMeasurements = product.productMeasurements.Select(m => m.measurement).ToList();

            foreach (Measurement measurement in possibleMeasurements)
            {
                double height = customizedDimensions.height;
                double width = customizedDimensions.width;
                double depth = customizedDimensions.depth;

                bool hasDimensionValues = measurement.hasValues(height, width, depth);

                if (hasDimensionValues)
                {
                    return; //return immediately if all the values match
                }
            }

            throw new ArgumentException(CUSTOMIZED_PRODUCT_DIMENSIONS_NOT_MATCHING_SPECIFICATION);
        }

        /// <summary>
        /// Checks if a given string is valid
        /// </summary>
        /// <param name="obj">String to check</param>
        private void checkString(string obj, string message)
        {
            if (String.IsNullOrEmpty(obj)) throw new ArgumentException(message);
        }

        /// <summary>
        /// Returns the CustomizedProduct's identity
        /// </summary>
        /// <returns>String with the CustomizedProduct's identity</returns>
        public string id()
        {
            return reference;
        }

        /// <summary>
        /// Checks if the CustomizedProduct's identity is the same as the one received as a parameter
        /// </summary>
        /// <param name="comparingEntity">Entity to compare to the CustomizedProduct</param>
        /// <returns>true if the given identifier is equal to the CustomizedProduct's identity; false, otherwise.</returns>
        public bool sameAs(string comparingEntity)
        {
            return reference.Equals(comparingEntity, StringComparison.InvariantCultureIgnoreCase);
        }


        public override bool activate()
        {
            if (activated)
            {
                return false;
            }

            activated = true;

            foreach (Slot slot in this.slots)
            {
                foreach (CustomizedProduct customizedProduct in slot.customizedProducts)
                {
                    return customizedProduct.activate();
                }
            }

            return true;
        }

        public override bool deactivate()
        {
            if (!activated)
            {
                return false;
            }

            activated = false;

            foreach (Slot slot in this.slots)
            {
                foreach (CustomizedProduct customizedProduct in slot.customizedProducts)
                {
                    return customizedProduct.deactivate();
                }
            }

            return true;
        }

        /// <summary>
        /// Returns a textual description of the CustomizedProduct
        /// </summary>
        /// <returns>String that describes the CustomizedProduct</returns>
        public override string ToString()
        {
            return string.Format("Designation: {0}, Reference {1}", designation, reference);
        }

        /// <summary>
        /// Returns the generated hash code of the CustomizedProduct
        /// </summary>
        /// <returns>Generated hash code</returns>
        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = hashCode * 23 + this.reference.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Checks if a certain CustomizedProduct is the same as a received object
        /// </summary>
        /// <param name="obj">Object to compare with the CustomizedProduct</param>
        /// <returns>true if both objects are equal, false if not</returns>
        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                CustomizedProduct other = (CustomizedProduct)obj;

                return this.reference.Equals(other.reference);
            }
        }

        /// <summary>
        /// Returns the current CustomizedProduct as a DTO
        /// </summary>
        /// <returns>CustomizedProductDTO with the current representation of the CustomizedProduct</returns>
        public CustomizedProductDTO toDTO()
        {
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
        public enum CustomizationStatus
        {
            PENDING,
            FINISHED
        }

        /// <summary>
        /// CustomizedProduct's Builder.
        /// </summary>
        public class CustomizedProductBuilder
        {
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
            public static CustomizedProductBuilder createCustomizedProduct(string reference, Product product, CustomizedDimensions customizedDimensions)
            {
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
            public static CustomizedProductBuilder createCustomizedProduct(string authToken, string reference, Product product, CustomizedDimensions customizedDimensions)
            {
                CustomizedProductBuilder builder = new CustomizedProductBuilder();
                builder.customizedProduct = new CustomizedProduct(authToken, reference, product, customizedDimensions);
                return builder;
            }

            /// <summary>
            /// Adds a designation to the CustomizedProduct being built.
            /// </summary>
            /// <param name="designation">The CustomizedProduct's designation.</param>
            /// <returns>An instance of CustomizedProductBuilder.</returns>
            public CustomizedProductBuilder withDesignation(string designation)
            {
                this.customizedProduct.changeDesignation(designation);
                return this;
            }

            /// <summary>
            /// Adds an instance of CustomizedMaterial to the CustomizedProduct being built.
            /// </summary>
            /// <param name="customizedMaterial">Instance of CustomizedMaterial that will be assigned to the built CustomizedProduct.</param>
            /// <returns>An instance of CustomizedProductBuilder.</returns>
            public CustomizedProductBuilder withMaterial(CustomizedMaterial customizedMaterial)
            {
                this.customizedProduct.changeCustomizedMaterial(customizedMaterial);
                return this;
            }

            /// <summary>
            /// Builds the instance of CustomizedProduct.
            /// </summary>
            /// <returns>Built instance of CustomizedProduct</returns>
            public CustomizedProduct build()
            {
                return this.customizedProduct;
            }
        }
    }
}