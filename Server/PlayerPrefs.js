class PlayerPrefs{
    data;
    constructor(){
        this.data = {};
    }

    set(key, value){
        data[key] = value;
    }

    GetValue(key){
        return data[key];
    }

    SetInt(key, value){
        set(key, value);
    }
    
    GetInt(key){
        return GetValue(key);
    }
}
module.exports = PlayerPrefs;