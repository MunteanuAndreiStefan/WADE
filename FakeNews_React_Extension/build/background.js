const header = document.getElementsByClassName('css-1dbjc4n r-1d09ksm r-18u37iz r-1wbh5a2');
const time_url = document.getElementsByClassName('css-4rbku5 css-18t94o4 css-901oao r-1re7ezh r-1loqt21 r-1q142lx r-1qd0xha r-a023e6 r-16dba41 r-ad9z0x r-bcqeeo r-3s2u2q r-qvutc0');

setTimeout(() => {
    console.log(header.length);
    for (let i = 0; i < header.length; i++) {
        console.log("Here");

        const xhr = new XMLHttpRequest();
        xhr.addEventListener("readystatechange", function() {
            // console.log(this.responseText);
            if(this.readyState === 4) {

                procent_1();
                procent_2();
                procent_3();
                procent_4();

                if (JSON.parse(this.responseText).credibilityScore > 50) {
                    header[i].appendChild(createBadge(true, JSON.parse(this.responseText).credibilityScore));
                } else {
                    header[i].appendChild(createBadge(false, JSON.parse(this.responseText).credibilityScore));
                }
            }
        });
        console.log(time_url[i].getAttribute("href"));
        xhr.open("GET", "https://fakenewsdetection.azurewebsites.net/api/NewsArticles/url?url=https://twitter.com"+time_url[i].getAttribute("href"));
        xhr.setRequestHeader("X-Api-Key", "FA872702-6396-45DC-89F0-FC1BE900591B");
        xhr.send();

        const url_vote = "https://twitter.com"+time_url[i].getAttribute("href")

        header[i].appendChild(createVoteTrue(url_vote));
        header[i].appendChild(createVoteFalse(url_vote));
    }
}, 2000);

function procent_1() {
    const data2 = new FormData();
    data2.append("url", "https://twitter.com/realDonaldTrump/status/1222331749805580293");
    const xhr1 = new XMLHttpRequest();
    xhr1.addEventListener("readystatechange", function() {
        if(this.readyState === 4) {
            console.log(this.responseText);
        }
    });
    xhr1.open("GET", "http://127.0.0.1:5000/procent_1");
    xhr1.setRequestHeader("Content-Type", "multipart/form-data");

    xhr1.send(data2);
}

function procent_2() {
    const data3 = new FormData();
    data3.append("url", "https://twitter.com/realDonaldTrump/status/1222331749805580293");
    const xhr = new XMLHttpRequest();
    xhr.addEventListener("readystatechange", function() {
        if(this.readyState === 4) {
            console.log(this.responseText);
        }
    });
    xhr.open("GET", "http://127.0.0.1:5000/procent_2");
    xhr.setRequestHeader("Content-Type", "multipart/form-data");
    xhr.send(data3);
}

function procent_3() {
    var data = new FormData();
    data.append("url", "https://twitter.com/realDonaldTrump/status/1222331749805580293");
    var xhr = new XMLHttpRequest();
    xhr.addEventListener("readystatechange", function() {
        if(this.readyState === 4) {
            console.log(this.responseText);
        }
    });
    xhr.open("GET", "http://127.0.0.1:5000/procent_2");
    xhr.setRequestHeader("Content-Type", "multipart/form-data");
    xhr.send(data);
}

function procent_4() {
    const data = new FormData();
    data.append("url", "https://twitter.com/realDonaldTrump/status/1222331749805580293");
    const xhr = new XMLHttpRequest();
    xhr.addEventListener("readystatechange", function() {
        if(this.readyState === 4) {
            console.log(this.responseText);
        }
    });
    xhr.open("GET", "http://127.0.0.1:5000/procent_4");
    xhr.setRequestHeader("Content-Type", "multipart/form-data");
    xhr.send(data);
}

function createBadge(bool, credibilityScore) {
    let status = document.createElement('span');
    if (bool) {
        status.innerHTML = "Fake: " + credibilityScore ;
        // CSS Styling Neutral
        status.style.padding = '5px 10px 5px 10px';
        status.style.marginLeft = "10px";
        status.style.borderRadius = "25px";
        status.style.backgroundColor = "#861b31";
        status.style.color = "#ffffff";
    } else {
        status.innerHTML = "Neutral: " + credibilityScore ;
        // CSS Styling Neutral
        status.style.padding = '5px 10px 5px 10px';
        status.style.marginLeft = "10px";
        status.style.borderRadius = "25px";
        status.style.backgroundColor = "#657786";
        status.style.color = "#ffffff";
    }
    return status;
}

function createVoteTrue(url) {
    let status = document.createElement('button');
    status.addEventListener("click", function () {

        const data = JSON.stringify({"applicationUserId":"test@gmail.com","newsArticleUrl": url,"isTrue":true});
        const xhr = new XMLHttpRequest();
        xhr.addEventListener("readystatechange", function() {
            if(this.readyState === 4) {
                console.log(this.responseText);
            }
        });
        xhr.open("POST", "https://fakenewsdetection.azurewebsites.net/api/Votes");
        xhr.setRequestHeader("X-Api-Key", "FA872702-6396-45DC-89F0-FC1BE900591B");
        xhr.setRequestHeader("Content-Type", "application/json");
        xhr.send(data);

        alert("Voted True !");
    });
    status.innerHTML = "👍";
    // CSS Styling Neutral
    status.style.padding = '5px 10px 5px 10px';
    status.style.marginLeft = "4px";
    status.style.borderRadius = "2px";
    status.style.backgroundColor = "#ffffff";
    status.style.color = "#ffffff";

    return status;
}

function createVoteFalse(url) {
    let status = document.createElement('button');
    status.addEventListener("click", function () {

        const data = JSON.stringify({"applicationUserId":"test@gmail.com","newsArticleUrl": url,"isTrue":false});
        const xhr = new XMLHttpRequest();
        xhr.addEventListener("readystatechange", function() {
            if(this.readyState === 4) {
                console.log(this.responseText);
            }
        });
        xhr.open("POST", "https://fakenewsdetection.azurewebsites.net/api/Votes");
        xhr.setRequestHeader("X-Api-Key", "FA872702-6396-45DC-89F0-FC1BE900591B");
        xhr.setRequestHeader("Content-Type", "application/json");
        xhr.send(data);

        alert("Voted False !");
    });
    status.innerHTML = "👎";
    // CSS Styling Neutral
    status.style.padding = '5px 10px 5px 10px';
    status.style.marginLeft = "1px";
    status.style.borderRadius = "2px";
    status.style.backgroundColor = "#ffffff";
    status.style.color = "#ffffff";
    return status;
}
