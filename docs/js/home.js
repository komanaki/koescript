const app = document.querySelector('pre');

const typewriter = new Typewriter(app, {
    loop: true,
    delay: 20
});

const codeLegend = document.querySelector("#codelegend");

const setLegend = (text) => {
    codeLegend.innerHTML = text;
    codeLegend.style.animation = "";
    codeLegend.offsetHeight;
    codeLegend.style.animation = "blink 1s linear forwards";
};

const lines = {
    "<span style='color: #51B1FF;'>@hour</span> 16:41" : () => {
        document.querySelector("#screen #hour").style.display = "block";
        setLegend("Set the hour");
    },
    "\n<span style='color: #51B1FF;'>@place</span> \"Momochi beach\" <span style='color: #D45FF6;'>city</span>=\"Fukuoka\"" : () => {
        document.querySelector("#screen #place").style.display = "block";
        document.querySelector("#screen #city").style.display = "block";
        setLegend("Set the place & the city");
    },
    "\n\n<span style='color: #51B1FF;'>@bg</span> \"fukuoka_momochi\" <span style='color: #D45FF6;'>translate</span>=\"down\"" : () => {
        document.querySelector("#screen").style.backgroundImage = "url('img/fukuoka_momochi.jpg')";
        document.querySelector("#screen").style.animation = "translateDown 30s linear forwards";
        setLegend("Show the background, animate it towards the bottom");
    },
    "\n\n<span style='color: #51B1FF;'>@cs</span> \"guide\" <span style='color: #D45FF6;'>state</span>=\"speaking\"" : () => {
        document.querySelector("#screen #chara").src = "img/guide_speaking.png";
        document.querySelector("#screen #chara").style.display = "block";
        setLegend("Show a character sprite");
    },
    "\nguide : Welcome to this KoeScript example." : () => {
        document.querySelector("#screen #message p").innerHTML = "Welcome to this KoeScript example.";
        document.querySelector("#screen #message").style.display = "block";
        setLegend("Display a dialogue message from the character \"guide\"");
    },
    "\n\n<span style='color: #51B1FF;'>@cs</span> \"guide\" <span style='color: #D45FF6;'>state</span>=\"smile\"" : () => {
        document.querySelector("#screen #chara").src = "img/guide_happy.png";
        setLegend("Change the character sprite");
    },
    "\nNice to meet you !" : () => {
        document.querySelector("#screen #message p").innerHTML = "Nice to meet you !";
        setLegend("Display another message from the same character");
    },
    "\n~ Take your time !" : () => {
        document.querySelector("#screen #sidemessage").innerHTML = "Take your time !";
        document.querySelector("#screen #sidemessage").style.display = "block";
        setLegend("Display a short message on the side");
    },
};

const resetState = () => {
    document.querySelector("#screen #hour").style.display = "none";
    document.querySelector("#screen #place").style.display = "none";
    document.querySelector("#screen #city").style.display = "none";
    document.querySelector("#screen").style.backgroundImage = "none";
    document.querySelector("#screen").style.animation = "";
    document.querySelector("#screen #chara").style.display = "none";
    document.querySelector("#screen #message").style.display = "none";
    document.querySelector("#screen #sidemessage").style.display = "none";
    setLegend("&nbsp;");
}

for (const [line, action] of Object.entries(lines)) {
    typewriter
        .typeString(line)
        .callFunction(action)
        .pauseFor(2000);
}

typewriter
    .pauseFor(5000)
    .callFunction(resetState)
    .deleteAll(1)
    .start();
