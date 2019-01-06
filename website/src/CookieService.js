
/**
 * Returns the current MYCA session token
 */
export const getMYCASessionToken=()=>{
    let cookies=document.cookie.match("(.*)MYCASESSION=(.+)(.*)");
    return cookies && cookies[2] ? cookies[2] : null;
};
