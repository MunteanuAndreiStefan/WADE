const id = document.getElementsByClassName("user-id")[0];
const random_id = '_' + Math.random().toString(36).substr(2, 9);
id.innerHTML = random_id;
