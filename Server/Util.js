exports.deleteArrV = (arr, _value) =>{
    if(arr == undefined)
         return [];
    var result = arr.filter(function(value, index, arr){ 
        return value != _value;
    });
    return result;
}

exports.deleteArrK = (arr, _index) =>{
    if(arr == undefined)
        return [];
    if(!Array.isArray(arr)){
        return [];
    }
    var result = arr.filter(function(value, index, arr){ 
        return index != _index;
    });
    return result;
}