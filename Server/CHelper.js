
exports.Shuffle = (list) =>
    { 
        var n = list.length;
        while (n > 1)
        {
            n--;
            //var k = rng.Next(n + 1);
            var k =  Math.floor(Math.random() * n);
            var value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        // function shuffle(arr){
        //     arr.sort(() => Math.random() - 0.5);
        // }
        // shuffle(list);
    }
