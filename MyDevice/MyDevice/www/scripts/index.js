(function () {
    "use strict";

    document.addEventListener( 'deviceready', onDeviceReady.bind( this ), false );

    function onDeviceReady() {
        // Handle the Cordova pause and resume events
        document.addEventListener( 'pause', onPause.bind( this ), false );
        document.addEventListener('resume', onResume.bind(this), false);
        window.addEventListener("batterystatus", onBatteryStatus, false);

        applyDeviceData();
        checkConnection();
    };

    function onPause() {
        // TODO: This application has been suspended. Save application state here.
    };

    function onResume() {
        // TODO: This application has been reactivated. Restore application state here.
    };

    function applyDeviceData() {
        $('#cordovaVersion').text(device.cordova);
        $('#manufacturer').text(device.manufacturer);
        $('#isVirtual').text(device.isVirtual);
        $('#operatingSystem').text(device.platform);
        $('#deviceModel').text(device.model);
        $('#osVersion').text(device.version);
        $('#uuid').text(device.uuid);
        $('#serial').text(device.serial);
    }

    function onBatteryStatus(status) {
        $('#batteryStatus').text(status.level);
        $('#isPlugged').text(status.isPlugged);
    }

    function checkConnection() {
        var networkState = navigator.connection.type;

        var states = {};
        states[Connection.UNKNOWN] = 'Unknown connection';
        states[Connection.ETHERNET] = 'Ethernet connection';
        states[Connection.WIFI] = 'WiFi connection';
        states[Connection.CELL_2G] = 'Cell 2G connection';
        states[Connection.CELL_3G] = 'Cell 3G connection';
        states[Connection.CELL_4G] = 'Cell 4G connection';
        states[Connection.CELL] = 'Cell generic connection';
        states[Connection.NONE] = 'No network connection';

        $('#connectionType').text(states[networkState]);
    }

} )();