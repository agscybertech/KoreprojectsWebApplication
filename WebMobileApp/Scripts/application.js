function isTouchDevice() {
  //return !!('ontouchstart' in window);
  return true;
}

$(document).ready(function(){
  if(!isTouchDevice()){
    //Mouse powered calendar
    //$('.datetimepicker').datetimepicker({});
    $('.datetimepicker').timepicker({});

    //Disable the user from manually entering values on the text field
    $('.datetimepicker').bind('keydown',function(e){
      $(this).attr('readonly', 'readonly');
      //Extra fun for working with backspace
      //we don't want the browser to go back in history if backspace is pressed
      if(e.keyCode == 8){
        e.preventDefault();
      }
    });

    //Remove readonly on keyup, so the form doesn't look weird 
    $('.datetimepicker').bind('keyup',function(){
      $(this).removeAttr('readonly');
    });


  }else{
    //Touch powered calendar
    $('.datetimepicker').scroller({
      //preset: 'datetime',
      preset: 'time',
      theme: 'android',
      display: 'modal',
      mode: 'scroller',
      width:100
      //ampm: false,
      //timeFormat: 'HH:ii',
      //timeWheels: 'HHii'
    }); 
  }
});
