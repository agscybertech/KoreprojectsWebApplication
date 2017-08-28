//'use strict';
window.START=function() {
	if(STYLE) {
		window.STYLE=undefined;
		window.START=undefined;
	}
};

// If browser (old browser) not support Array.indexOf – make it
(function(){
	if(!Array.indexOf){
		Array.prototype.indexOf=function(obj){
			for(var i=0;i<this.length;i=i+1){
				if(this[i]===obj){ return i; }
			}
			return -1;
		};
	}
})();



//--------------------------------------------------------------------------------------------------
// Global Config object
// Use for store static value such as URLs or max/min value or some parametars for server or etc.
//--------------------------------------------------------------------------------------------------
window.CONFIG={
    getTemplateURL: function () { return 'Templates.ashx'; },
    gatMasterURL: function () { return 'master.ashx'; },
	gatMessagesURL:function(){ return 'messages.txt'; },
	gatSendingURL:function(){ return 'msg-sending.php'; },
	gatSendingHoursOfDay: function () { return 'MobileService.asmx/Greetings'; },
	gatSyncTime:function(){ return 10000; },
	gatTransitionDuration:function(){ return 300; }	// Depends on the CSS duration
};



//--------------------------------------------------------------------------------------------------
// Create Angular application
//--------------------------------------------------------------------------------------------------
var App=angular.module('App', []);



//--------------------------------------------------------------------------------------------------
// $ TEMPLATE CACHE SERVICE
// This is a srvice for loading all views/pages templates as one file
//--------------------------------------------------------------------------------------------------
App.factory('$templateCache', ['$cacheFactory', '$http', '$injector', function($cacheFactory, $http, $injector) {
	var cache = $cacheFactory('templates');
	var allTplPromise;

	return {
	get: function(url) {
		var fromCache = cache.get(url);
		if(fromCache) {
			return fromCache;}
		if(!allTplPromise) {
			allTplPromise = $http.get( CONFIG.getTemplateURL() ).then(function(response) {
				$injector.get('$compile')(response.data);
				return response;
			});
		}
		return allTplPromise.then(function(response) {
			return {
			status: response.status,
			data: cache.get(url)
			};
		});
	},
	put: function(key, value) {
			cache.put(key, value);}
	};
}]);



//--------------------------------------------------------------------------------------------------
// $ TIME SHEET
//--------------------------------------------------------------------------------------------------
App.factory('$timesheet', [function(){
	var dayName=['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
		weeks=[],
		closed=[],
		selectedWeek=0,
		selectedDay=0;

	// Set data
	function update(d) {
		weeks=d.weeks;
		selectedWeek=weeks.length-1;
		for(var i=0; i<weeks.length; i+=1){
			closed.push(weeks[i].pop());}
	}

	// Get week and set as a current week
	function getWeek(n) {
		n=n || 0;
		selectedWeek+=n;
		selectedWeek= selectedWeek<0 ? weeks.length-1:selectedWeek;
		selectedWeek= selectedWeek>=weeks.length ? 0:selectedWeek;
		return weeks[selectedWeek]; }

	// Get flag of closed status for current week
	function isClosed() {
		return closed[selectedWeek]; }

	// Get dada of day
	function getDay(n) {
		n=n || 0;
		selectedDay=n;
		return weeks[selectedWeek][selectedDay];
	}

	// Set dada of day
	function setDay(h) {
		weeks[selectedWeek][selectedDay].value=h.total;
		weeks[selectedWeek][selectedDay].hours.s=h.start;
		weeks[selectedWeek][selectedDay].hours.f=h.finish;
		weeks[selectedWeek][selectedDay].hours.sl=h.startLunch;
		weeks[selectedWeek][selectedDay].hours.fl=h.finishLunch;
	}

	function convertTime(s) {
	    //var space = s.indexOf(" ");
	    //if (space > 0) {
	    //    var apt = s.toString().split(' ');
	    //    if (apt.length === 2) {
	    //        var ap = apt[1];
	    //        var t = apt[0].toString().split(':'),
		//	    r = 0,
		//	    h = 0,
		//	    m = 0;
	    //        if (t.length === 1) { t.push('0'); }
	    //        h = parseInt(t[0]);
	    //        if (ap === 'am') {
	    //            if (h === 12) {
	    //                h = 0;
	    //            }
	    //        } else {
	    //            if (h=12){
	                
	    //            }else{
	    //                h = h + 12;
	    //            }  
	    //        }
	    //        m = parseInt(t[1]);
	    //        h = h < 0 ? 0 : h;
	    //        h = h > 23 ? 23 : h;
	    //        m = m < 0 ? 0 : m;
	    //        m = m > 59 ? 59 : m;
	    //        r = m / 60;
	    //        r = h + r;
	    //        return r;
	    //    } else {
	    //        t.push('0');
	    //    } 
	    //} else {
	        var t = s.toString().split(':'),
			r = 0,
			h = 0,
			m = 0;
	        if (t.length === 1) { t.push('0'); }
	        h = parseInt(t[0]);
	        m = parseInt(t[1]);
	        h = h < 0 ? 0 : h;
	        h = h > 23 ? 23 : h;
	        m = m < 0 ? 0 : m;
	        m = m > 59 ? 59 : m;
	        r = m / 60;
	        r = h + r;
	        return r;
	    //}
	};

	function roundTime(t) {
		var h=0, m=0;
		t=parseFloat(t);
		t=t.toFixed(2);
		h=parseInt(t);
		m=parseInt((t-h)*60);
		m=m<0?'0'+m:''+m;
		return h+':'+m;
	};

	// Public:
	return {
		convertTime:convertTime,
		roundTime:roundTime,
		update:update,
		getWeek:getWeek,
		isClosed:isClosed,
		getDay:getDay,
		setDay:setDay
	};
}]);



//--------------------------------------------------------------------------------------------------
// $ MESSAGING SERVICE
// The service is intended to maintain the messaging operation (loading, sending, update messages, etc.).
// This service works non-stop, even when the message is not show. 
//--------------------------------------------------------------------------------------------------
App.factory('$messaging', ['$http', '$timeout', function($http, $timeout){
	var data=[''],
		updateFun=null;

	// Load message as JSON and call callback function (optional)
	function load(callback) {
		$http.get(CONFIG.gatMessagesURL()).
		success(function(d) {
			data=d;

			if(typeof updateFun==='function') {
				updateFun(d);}

			if(typeof callback==='function') {
				callback();}
		});
	};

	// Start the synchronization
	function sync() {
		$timeout(function(){
			load(sync);
		}, CONFIG.gatSyncTime());
	};

	function update(f) {
		updateFun=f;
		return data;
	};

	// Load messages for the first time
	load();


	// Public:
	return {
		getData:update,
		setData:data,
		startSync:function() {
			load(sync);}
	};
}]);



//--------------------------------------------------------------------------------------------------
// Messages DIRECTIVE
// This is as a component for messages (showing, reading, etc)
//--------------------------------------------------------------------------------------------------
App.directive('messages', function($q, $http, $timeout, $messaging) {
	return {
		replace:true,
		restrict:'A',
		link:function(scope, element, attrs) {
			var msgs={
					data:[''],
					len:0,
					curr:0
				};

			function update(d){
				msgs.data=d;
				msgs.len=d.length;
			};

			msgs.data=$messaging.getData(update);
			msgs.len=msgs.data.length;

			// Get current message 
			scope.getMsg=function() {
				if(msgs.len===0) {
					scope.toggle=false;
					return ''; }
				return msgs.data[msgs.curr].msg;
			};

			// Get current message sender
			scope.getMsgFrom=function() {
				if(msgs.len===0) { return ''; }
				return msgs.data[msgs.curr].from;
			};

			// Delete current messege
			scope.deleteMsg=function() {
				if(msgs.len===0) { return; }
				msgs.data.splice(msgs.curr,1);
				msgs.len-=1;
				scope.toggle=msgs.len===0?false:scope.toggle;
				msgs.curr=msgs.curr===msgs.len?msgs.curr-1:msgs.curr;
				msgs.curr=msgs.curr<0?0:msgs.curr;
			};

			// Get previous message
			scope.prev=function() {
				var n=msgs.curr,
					l=msgs.len;
				msgs.curr=n<=0?l-1:n-1;};

			// Get next message
			scope.next=function() {
				var n=msgs.curr,
					l=msgs.len;
				msgs.curr=n>=l-1?0:n+1;};

			// Go to write messages page
			scope.writeMsg=function() {
				scope.goTo('sending/');
			};

			// Go to write messages page for reply
			scope.replyMsg=function() {
				scope.goTo('sending/'+msgs.data[msgs.curr].from);
			};
		},
		templateUrl:'messages.html'
	};
});



//--------------------------------------------------------------------------------------------------
// Touch Button DIRECTIVE
// This is a fix for touch action on buttons. Work only on elements with btn class
//--------------------------------------------------------------------------------------------------
App.directive('btn', function() {
	return {
		replace:false,
		restrict:'C',
		link:function(scope, element) {
			element.bind('touchstart', function() {
				element.addClass('touch');});
			element.bind('touchend', function() {
				element.removeClass('touch');});
		}
	};
});



//--------------------------------------------------------------------------------------------------
// R O U T I N G
//--------------------------------------------------------------------------------------------------
App.config(function($routeProvider, $locationProvider) {
	$locationProvider.hashPrefix("!");
	$locationProvider.html5Mode(false);
	$routeProvider.
		when('/',				{templateUrl:'mainmenu.html'}).
		when('/sending/:to',	{templateUrl:'sending.html'}).
		when('/timesheet',		{templateUrl:'timesheet.html'}).
		when('/timesheet-edit/:day',{templateUrl:'timesheet-edit.html'}).
		when('/jobs',			{templateUrl:'jobs.html'}).
		when('/photos',			{templateUrl:'photos.html'}).
		when('/reorder',		{templateUrl:'reorder.html'}).
		when('/servererror', { template: '<section class="open"><h1 class="appmsg">Error:<br>No server connection</h1></section>' }).
        otherwise(				{redirectTo:'/'});
});



//--------------------------------------------------------------------------------------------------
// App CONTROLLER
// Controller for application or main controller. In this controller, we start the application and
// control animation for replacing views/pages.
//--------------------------------------------------------------------------------------------------
App.controller('AppCtrl', function($scope, $location, $rootScope, $timeout, $http, $messaging, $timesheet) {
	START();
	$rootScope.loaded=true;		// This hide loader and show header, footer, pages...
	$rootScope.state = '';		// State of view/page (open, close)
	$rootScope.navigation=true;	// Showing navigation
	$scope.history=[''];		// Need only for back function

	//$scope.company_id='kore';	// Company id
	//$scope.user_id='chris';		// user id

	$rootScope.userData={};		// Compleat user data

	// Load user data
	function getMaster() {
	    $http({method:'GET', url:CONFIG.gatMasterURL()+'?company='+$scope.company_id+'&user='+$scope.user_id}).
		success(function(data) {
		    $rootScope.userData = data;
		    if (data.user.length > 10) {
		        $scope.user_name = data.user.substring(0,10) + '...';
		    }else{
		        $scope.user_name = data.user;
	        }
		    $scope.user_id = data.userid;
		    if (data.company.length > 10) {
		        $scope.company_name = data.company.substring(0, 10) + '...';
		    } else {
		        $scope.company_name = data.company;
		    }
		    $scope.company_id = data.companyid;
			$timesheet.setData=data.messages;
			$messaging.startSync();		// Start messaging synchronization
			$timesheet.update(data);
		}).
		error(function() {
			$scope.goTo('servererror');
		});
	};
	getMaster();

	// Open new view/page
	$scope.goTo=function(src) {
		if(src===$scope.history[$scope.history.length-1]) { return; }

		// Closing dropdown menu in header
		$scope.toggleC=$scope.toggleW=false;

		$rootScope.state='close';
		$timeout(function(){
			$rootScope.state='';

			if(src==='back') {
				$scope.history.pop();
				src=$scope.history.pop();}

			$scope.history.push(src);
			$location.path(src).replace();
			$rootScope.$apply();
		}, CONFIG.gatTransitionDuration());
		$rootScope.navigation=true;
	};

	// Opening page when page is ready
	$scope.ready=function(){
		$rootScope.state='';
		$timeout(function(){
			$rootScope.state='open';
			$rootScope.$apply();
		}, 50);
	};

	$scope.logout = function () {
	    //$location.path("/logout");
	    window.location.replace("/login.aspx?act=Xl/LUMvkln8=");
	};
});



//--------------------------------------------------------------------------------------------------
// Main Menu CONTROLLER
// Home page
//--------------------------------------------------------------------------------------------------
App.controller('MainMenuCtrl', function($scope, $rootScope) {
	$scope.ready();
	$rootScope.navigation=false;
});



//--------------------------------------------------------------------------------------------------
// Sending Message CONTROLLER
// When called this view/page (via URL) you send a address of the sender (eg sending/chris)
// and this page go to "Reply mode". If you call this without address (eg sending/),
// page go to "New message mode"
//--------------------------------------------------------------------------------------------------
App.controller('SendingCtrl', function($scope, $rootScope, $routeParams, $http, $timeout) {
	$scope.ready();
	$rootScope.navigation=false;

	$scope.msg='';
	$scope.finish=false;
	$scope.sendingTo=$routeParams.to;
	$scope.reply=$routeParams.to===''?false:true;

	// Sending messagess
	$scope.sendMsg=function() {
		$scope.sendingTo;	// Address of the sender
		$scope.textMsg;		// Message

		$scope.process=true;
		// Real sending process
		/*
		$http({method:'GET', url:CONFIG.gatSendingURL()+'?'+$scope.sendingTo+'&'+$scope.textMsg}).
		success(function(data) {
			$scope.msg=data;	// Back info of sending (eg 'Message has been sent' or some error message)

			// Show message and after 0.5 sec goto Home page
			$scope.finish=true;
			$timeout(function() {
				$scope.goTo('');
			}, 500);
		}).
		error(function() {
			$scope.msg='Sending error';

			// Show message and after 0.5 sec goto Home page
			$scope.finish=true;
			$timeout(function() {
				$scope.goTo('');
			}, 500);
		});
		*/

		// Simulation of sending messages
		$timeout(function() {
			$scope.msg='Message has been sent';

			// Show message and after 0.5 sec goto Home page
			$scope.finish=true;
			$timeout(function() {
				$scope.goTo('home');
			}, 500);
		}, 1500);
	};
});



//--------------------------------------------------------------------------------------------------
// Time Sheet CONTROLLER
//--------------------------------------------------------------------------------------------------
App.controller('TimeSheetCtrl', function($scope, $timesheet) {
    $scope.ready();

	$scope.day=$timesheet.getWeek(0);
	$scope.closed=$timesheet.isClosed();

	$scope.getTotal=function(){
		var r=0;
		for(var i=0;i<7;i=i+1) {
			r+=$timesheet.convertTime($scope.day[i].value);}
		return $timesheet.roundTime(r); };

	$scope.getNextPayday=function(){
		var a=$scope.day[0].date.split('/'),
			d=new Date(a[2], parseInt(a[1])-1, parseInt(a[0])+7),
			r=d.getDate() +'/'+ (parseInt(d.getMonth())+1) +'/'+ d.getFullYear();
		return r; };

	$scope.getPrevWeek=function(){
		$scope.day=$timesheet.getWeek(-1);
		$scope.closed=$timesheet.isClosed(); };
	$scope.getNextWeek=function(){
		$scope.day=$timesheet.getWeek(1);
		$scope.closed=$timesheet.isClosed(); };
});



//--------------------------------------------------------------------------------------------------
// Time Sheet Edit CONTROLLER
//--------------------------------------------------------------------------------------------------
App.controller('TimeSheetEditCtrl', function ($scope, $routeParams, $rootScope, $timeout, $http, $messaging, $timesheet) {
    $scope.ready();

    //OnClickGreetings();

	var day=$timesheet.getDay($routeParams.day),
		hours=day.hours;
	    dateDay = day.date;
	    dateName = day.name;
	$scope.start=hours.s;
	$scope.finish=hours.f;
	$scope.startLunch=hours.sl;
	$scope.finishLunch = hours.fl;
	$scope.dateDay = dateDay;
	$scope.dateName = dateName;
    //$scope.ViewSubTotal = 'true';
	$scope.ViewSubTotal = 'false';

	$scope.convertTime=$timesheet.convertTime;
	$scope.roundTime = $timesheet.roundTime;

	$scope.getTotal = function () {
	    //var r = $timesheet.convertTime(day.value);
	    var r = day.value;
	    if (r <= 0 || r === 'NaN') {
	        return '';
	    }
	    else {
	        $scope.ViewSubTotal = 'false';
	        //return $timesheet.roundTime(r);
	        return r;
	    }
	};

    /*
	$scope.getTotal=function(){
		var s=$timesheet.convertTime($scope.start),
			f=$timesheet.convertTime($scope.finish),
			sl=$timesheet.convertTime($scope.startLunch),
			fl=$timesheet.convertTime($scope.finishLunch),
			r=0;
		r = (f - s - (fl - sl));
		if (r < 0 || r === 'NaN') {
		    return '';}
		else {
		    //$scope.ViewSubTotal = 'true';
		    return $timesheet.roundTime(r);}
	};
    */

    // Load user data
	function getMaster() {
	    $http({ method: 'GET', url: CONFIG.gatMasterURL() + '?company=' + $scope.company_id + '&user=' + $scope.user_id }).
        success(function (data) {
            $rootScope.userData = data;
            $scope.user_name = data.user;
            $scope.user_id = data.userid;
            $scope.company_name = data.company;
            $scope.company_id = data.companyid;
            $timesheet.setData = data.messages;
            $messaging.startSync();		// Start messaging synchronization
            $timesheet.update(data);
        }).
        error(function () {
            $scope.goTo('servererror');
        });
	};

	$scope.done = function () {
	    /*	    getMaster();*/

		if(	$scope.start===hours.s &&
			$scope.finish===hours.f &&
			$scope.startLunch===hours.sl &&
			$scope.finishLunch===hours.fl
		) {
			$scope.goTo('timesheet');
			return; }
		$timesheet.setDay({
			total:$scope.getTotal(),
			start:$scope.start,
			finish:$scope.finish,
			startLunch:$scope.startLunch,
			finishLunch:$scope.finishLunch
		});		

	    // Real sending process
	    /*
		$http({method:'GET', url:CONFIG.gatSendingHoursOfDay()+'?'+
			'TimeSheetDate=' + dateDay +
			'&UserID=' + $scope.user_id +
            '&CompanyID=' + $scope.company_id +
			'&WorkStartTime=' + $scope.start +
			'&WorkEndTime' + $scope.finish +
			'&LunchStartTime' + $scope.startLunch +
			'&LunchEndTime' + $scope.finishLunch
		}).
		success(function(data) {
			$scope.goTo('timesheet'); }).
		error(function() {
			$scope.goTo('servererror');
		});
        */

	    // Simulation of sending hours
	    
		$timeout(function() {
			$scope.goTo('timesheet');
		}, 1000);
	};

    // Date Time Picker break the 'AngularJS magic', so need fix it
	function updateTime() {
	    $scope.start = $('input[ng-model="start"]').val();
	    $scope.finish = $('input[ng-model="finish"]').val();
	    $scope.startLunch = $('input[ng-model="startLunch"]').val();
	    $scope.finishLunch = $('input[ng-model="finishLunch"]').val();
	    $scope.$apply();
	};

	$('.datetimepicker').scroller({
	    //preset: 'datetime',
	    preset: 'time',
	    theme: 'android',
	    display: 'modal',
	    mode: 'scroller',
	    width: 100,
	    //ampm: false,
	    //timeFormat: 'HH:ii',
	    //timeWheels: 'HHii'
	    onSelect: updateTime
	});

	var WorkStart = $('#WorkStart');
	var LunchStart = $('#LunchStart');
	var LunchEnd = $('#LunchEnd');
	var WorkEnd = $('#WorkEnd');

	WorkStart.bind("change paste keyup", function () {
	    //$scope.start = $(this).val();
        //$scope.getTotal = function () {
        //    var s = $timesheet.convertTime($scope.start),
        //        f = $timesheet.convertTime($scope.finish),
        //        sl = $timesheet.convertTime($scope.startLunch),
        //        fl = $timesheet.convertTime($scope.finishLunch),
        //        r = 0;
        //    r = (f - s - (fl - sl));
        //    if (r < 0 || r === 'NaN') {
        //        return '';
        //    }
        //    else {
        //        return $timesheet.roundTime(r);
        //    }
	    //};

	    if ($scope.ViewSubTotal == 'true') {
	        LunchStart.val($(this).val());
	        LunchEnd.val($(this).val());
	        WorkEnd.val($(this).val());
	    }
	});

	

	LunchStart.bind("change paste keyup", function () {
	    //$scope.startLunch = $(this).val();
	    //$scope.getTotal = function () {
	    //    var s = $timesheet.convertTime($scope.start),
        //        f = $timesheet.convertTime($scope.finish),
        //        sl = $timesheet.convertTime($scope.startLunch),
        //        fl = $timesheet.convertTime($scope.finishLunch),
        //        r = 0;
	    //    r = (f - s - (fl - sl));
	    //    if (r < 0 || r === 'NaN') {
	    //        return '';
	    //    }
	    //    else {
	    //        return $timesheet.roundTime(r);
	    //    }
	    //};

	    if ($scope.ViewSubTotal == 'true') {
	        LunchEnd.val($(this).val());
	        WorkEnd.val($(this).val());
	    }
	});

	LunchEnd.bind("change paste keyup", function () {
	    //$scope.finishLunch = $(this).val();
	    //$scope.getTotal = function () {
	    //    var s = $timesheet.convertTime($scope.start),
        //        f = $timesheet.convertTime($scope.finish),
        //        sl = $timesheet.convertTime($scope.startLunch),
        //        fl = $timesheet.convertTime($scope.finishLunch),
        //        r = 0;
	    //    r = (f - s - (fl - sl));
	    //    if (r < 0 || r === 'NaN') {
	    //        return '';
	    //    }
	    //    else {
	    //        return $timesheet.roundTime(r);
	    //    }
	    //};

	    if ($scope.ViewSubTotal == 'true') {
	        WorkEnd.val($(this).val());
	    }
	});

	//WorkEnd.bind("change paste keyup", function () {
	//    $scope.finish = $(this).val();
	//    $scope.getTotal = function () {
	//        var s = $timesheet.convertTime($scope.start),
    //            f = $timesheet.convertTime($scope.finish),
    //            sl = $timesheet.convertTime($scope.startLunch),
    //            fl = $timesheet.convertTime($scope.finishLunch),
    //            r = 0;
	//        r = (f - s - (fl - sl));
	//        if (r < 0 || r === 'NaN') {
	//            return '';
	//        }
	//        else {
	//            return $timesheet.roundTime(r);
	//        }
	//    };
	//});
});



//--------------------------------------------------------------------------------------------------
// Jobs CONTROLLER
//--------------------------------------------------------------------------------------------------
App.controller('JobsCtrl', function($scope) {
	$scope.ready();
	$scope.appName='Jobs COMING SOON';
});



//--------------------------------------------------------------------------------------------------
// Photos CONTROLLER
//--------------------------------------------------------------------------------------------------
App.controller('PhotosCtrl', function($scope) {
	$scope.ready();
	$scope.appName='Photos COMING SOON';
});



//--------------------------------------------------------------------------------------------------
// Reorder CONTROLLER
//--------------------------------------------------------------------------------------------------
App.controller('ReorderCtrl', function($scope) {
	$scope.ready();
	$scope.appName='Reorder COMING SOON';
});