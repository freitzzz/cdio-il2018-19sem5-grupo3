/**
 * Constant that represents the small package size
 */
const SMALL={
"name":"S",
"dimensions":{
    "height":100,
    "width":100,
    "depth":100
}
};

/**
 * Constant that represents the medium package size
 */
const MEDIUM={
"name":"M",
"dimensions":{
    "height":150,
    "width":150,
    "depth":150
}
};

/**
 * Constant that represents the large package size
 */
const LARGE={
"name":"L",
"dimensions":{
    "height":200,
    "width":200,
    "depth":200
}
};

/**
 * Constant that represents the enum representation of the packages size
 */
const PackageSizeEnum={
    SMALL:SMALL,
    MEDIUM:MEDIUM,
    LARGE:LARGE,
    values:[SMALL,MEDIUM,LARGE]
}

/**
 * Exports PackageSize as an enum
 */
module.exports=PackageSizeEnum;