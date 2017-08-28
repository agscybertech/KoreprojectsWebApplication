var MobileServiceProxy;

// Initializes global and proxy default variables.
function pageLoad() {
    // Instantiate the service proxy.
    MobileServiceProxy = new Warpfusion.A4PP.WebServices.MobileService();

    // Set the default call back functions.
    MobileServiceProxy.set_defaultSucceededCallback(SucceededCallback);
    MobileServiceProxy.set_defaultFailedCallback(FailedCallback);
}


// Processes the button click and calls
// the service Greetings method.  
function OnClickGreetings() {
    var greetings = MobileServiceProxy.Greetings();
}

function AddTimeSheet(TimeSheetDate, UserID, CompanyID, WorkStartTime, LunchStartTime, LunchEndTime, WorkEndTime) {
    var TimeSheet = MobileServiceProxy.AddTimeSheet(TimeSheetDate, UserID, CompanyID, WorkStartTime, LunchStartTime, LunchEndTime, WorkEndTime);
}

// Callback function that
// processes the service return value.
function SucceededCallback(result) {
    //alert(result);
    //var RsltElem = document.getElementById("WorkStart");
    //RsltElem.value = result;
    //alert(RsltElem.value);
    //RsltElem = document.getElementById("Results");
    //RsltElem.innerHTML = result;
    //alert(RsltElem.innerHTML);
}

// Callback function invoked when a call to 
// the  service methods fails.
function FailedCallback(error, userContext, methodName) {
    if (error !== null) {
        var RsltElem = document.getElementById("Results");

        RsltElem.innerHTML = "An error occurred: " +
            error.get_message();
    }
}

if (typeof (Sys) !== "undefined") Sys.Application.notifyScriptLoaded();