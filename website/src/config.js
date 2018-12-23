let mycm, myco;
if (process.env.NODE_ENV === 'production') {
    mycm = "https://mycm-api.azurewebsites.net"
    myco = "http://cdio-myco.westeurope.cloudapp.azure.com"

} else {
    mycm = "http://localhost:5000/mycm/api"
    myco = "http://localhost:4000/myco/api"
}


const MYCM_API_URL = mycm;
const MYCO_API_URL = myco;
module.exports = { MYCM_API_URL, MYCO_API_URL };