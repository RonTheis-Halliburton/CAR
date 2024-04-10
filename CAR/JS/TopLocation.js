var localDev = "http://localhost:";
var corpHal = "corp.halliburton.com";
var prodExternal = "https://supplychain.halliburton.com/";
var prodInternal = "http://supplychain.corp.halliburton.com/";
var testInternal = "http://np2apph786v";
var prodName = "np1appn041v";
var prodDns = "demfgweb";
var stagingName = "np2apph788v";
var stagingDns = "supplychainqa";

if (top.location != self.location) {

    if (top.location.toString().indexOf(localDev) < 0
        && top.location.toString().indexOf(corpHal) < 0
        && top.location.toString().indexOf(prodExternal) < 0
        && top.location.toString().indexOf(prodInternal) < 0
        && top.location.toString().indexOf(testInternal) < 0
        && top.location.toString().indexOf(prodName) < 0
        && top.location.toString().indexOf(prodDns) < 0
        && top.location.toString().indexOf(stagingName) < 0
        && top.location.toString().indexOf(stagingDns) < 0
    ) {
        top.location = self.location.href;
    }
}