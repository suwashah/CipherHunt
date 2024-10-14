function getUserName(isAdmin=false) {
    var apiUrl= (isAdmin==true)?"/get_adusername":"/get_username"
    fetch(apiUrl).then(e => e.text()).then(e => {
        console.log("Username:", e)
    }
    ).catch(e => {
        console.error("Error fetching the username:", e)
    }
    )
}
