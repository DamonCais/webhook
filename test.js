



class api{
    hello(){
        console.log('hello');
    }
    hello(a){
        console.log('hello2'+a);
    }
}

let a = new api();
a.hello();
a.hello('a');