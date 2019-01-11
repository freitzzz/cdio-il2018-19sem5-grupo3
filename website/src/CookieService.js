
/**
 * Returns the current MYCA session token
 */
export const getMYCASessionToken=()=>{
    let cookies=document.cookie.match("(.*)MYCASESSION=(.+)(.*)");
    return cookies && cookies[2] ? cookies[2] : null;
};

/**
 * Deletes the current MYCA session cookie
 */
export const deleteMYCASessionCookie=()=>{
    document.cookie="MYCASESSION"+"=; Max-Age=-9999999;";
};
