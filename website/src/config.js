let mycm, myco, myca, privateMYCSSecreteChallenge;
if (process.env.NODE_ENV === 'production') {
    mycm = "https://mycm-api.azurewebsites.net";
    myco = "http://cdio-myco.westeurope.cloudapp.azure.com";
    myca = "http://myca-api.ukwest.cloudapp.azure.com/myca/api";
    privateMYCSSecreteChallenge="This secrete is for production only";

} else {
    mycm = "http://localhost:5000/mycm/api";
    myco = "http://localhost:4001/myco/api";
    myca = "http://localhost:8081/myca/api";
    privateMYCSSecreteChallenge="This secrete is for development only";
}

import Axios from 'axios';
Axios.defaults.headers['Secrete']=PRIVATE_MYCS_SECRETE_CHALLENGE;

export const MYCM_API_URL = mycm;
export const MYCO_API_URL = myco;
export const MYCA_API_URL = myca;
export const PRIVATE_MYCS_SECRETE_CHALLENGE=privateMYCSSecreteChallenge;