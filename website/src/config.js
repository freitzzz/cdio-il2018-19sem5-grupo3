let mycm, myco, myca;
if (process.env.NODE_ENV === 'production') {
    mycm = "https://mycm-api.azurewebsites.net";
    myco = "http://cdio-myco.westeurope.cloudapp.azure.com";
    myca = "https://myca.azurewebsites.net/myca/api";

} else {
    mycm = "http://localhost:5000/mycm/api";
    myco = "http://localhost:4000/myco/api";
    myca = "http://localhost:8081/myca/api";
}

export const MYCM_API_URL = mycm;
export const MYCO_API_URL = myco;
export const MYCA_API_URL = myca;