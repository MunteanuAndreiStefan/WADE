setTimeout(() => {
    const header = document.getElementsByClassName('css-1dbjc4n r-1d09ksm r-18u37iz r-1wbh5a2');
    console.log(header.length);
    const random = Math.floor(Math.random() * 5) + 1;
    for (let i = 0; i < header.length; i++) {
        // Set false alert
        if ( i === random ) { createRequest(); }
        else {
            // CSS Styling Neutral
            header[i].appendChild(createBadge(false));
        }
    }
}, 2000);

function createRequest() {
    let xhr = new XMLHttpRequest();
    xhr.withCredentials = true;
    xhr.addEventListener("readystatechange", function() {
        if(this.readyState === 4) {
            if (JSON.parse(this.responseText)[0].credibilityScore < 50) {
                header[i].appendChild(createBadge(true));
            } else header[i].appendChild(createBadge(false));
        }
    });
    xhr.open("GET", "https://fakenewsdetection.azurewebsites.net/api/newsarticles");
    xhr.send();
}

function createBadge(bool) {
    let status = document.createElement('span');
    if (bool) {
        status.innerHTML = "Fake";
        // CSS Styling Neutral
        status.style.padding = '5px 10px 5px 10px';
        status.style.marginLeft = "10px";
        status.style.borderRadius = "25px";
        status.style.backgroundColor = "#861b31";
        status.style.color = "#ffffff";
    } else {
        status.innerHTML = "Neutral";
        // CSS Styling Neutral
        status.style.padding = '5px 10px 5px 10px';
        status.style.marginLeft = "10px";
        status.style.borderRadius = "25px";
        status.style.backgroundColor = "#657786";
        status.style.color = "#ffffff";
    }
    return status;
}
