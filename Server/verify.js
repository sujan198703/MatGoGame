//var timespan = require("timespan");

/*
  Calculate crc32 of something.
*/
var crc32 = (function() {
    var c, crcTable = []; // generate crc table
    
    for (var n = 0; n < 256; n++) {
      c = n;
      
      for (var k = 0; k < 8; k++) {
        c = ((c & 1) ? (0xEDB88320 ^ (c >>> 1)) : (c >>> 1));
      }
      
      crcTable[n] = c;
    }
    
    return function(str) {
      var crc = 0 ^ (-1); // calculate actual crc
      
      for (var i = 0; i < str.length; i++) {
        crc = (crc >>> 8) ^ crcTable[(crc ^ str.charCodeAt(i)) & 0xFF];
      }
      
      return (crc ^ (-1)) >>> 0;
    }
  })();
  
  /*
    Generates a UUID based on the browser's capabilities.
  */
  function uuid() {
    var d = (Date.now !== undefined && typeof Date.now === "function") ? Date.now() : new Date().getTime();
    
    //if (window.performance && typeof window.performance.now === "function")
    //  d += performance.now();
    
    var uuid = "xxxxxxxx-xxxx-4xxxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function(c) {
      var r = (d + Math.random() * 16) % 16 | 0;
      d = Math.floor(d / 16);
      return (c == "x" ? r : (r & 0x3 | 0x8)).toString(16);
    });
    
    return uuid;
  }
  
  var crcPattern = "00000000";
  
exports.vcode = function vcode() {
  var code_pattern = "01234567890ABCDEFGHIJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
  var code = "";
  var d = (Date.now !== undefined && typeof Date.now === "function") ? Date.now() : new Date().getTime();
  for(i=0; i<8; i++) {
    d = Math.floor((d + Math.random() * 61) % 61);
    code += code_pattern[d];
  }
    return code;
}

Date.prototype.addHours = function(h) {    
    this.setTime(this.getTime() + (h*60*60*1000)); 
    return this;   
}

exports.expire_time = function expire_time(duration) {
    var d = (Date.now !== undefined && typeof Date.now === "function") ? Date.now() : new Date().getTime();
    //console.log(d);
    return new Date(d + (duration*1000)); 
}

exports.current_time = function current_time() {
    return (Date.now !== undefined && typeof Date.now === "function") ? Date.now() : new Date().getTime();
}