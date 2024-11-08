
const BASE_URL ="https://localhost:7259";

// Token'ı almak için fonksiyon
async function getToken(username,password) {
    const loginModel = {
        Username: username,
        Password: password
    };

    const response = await fetch(BASE_URL+'/api/Auth/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(loginModel)
    });

    if (!response.ok) {
        throw new Error('Login failed');
    }

    const data = await response.json();
    console.log("token: ",data.token);
    localStorage.setItem("token",data.token);
    return data.token;
}

// Korumalı API'ye istek göndermek için fonksiyon
async function fetchProtectedData(token) {
    const response = await fetch(BASE_URL+'/api/ToBuy', {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });

    if (!response.ok) {
        throw new Error('Failed to fetch protected data');
    }

    const data = await response.json();
    localStorage.setItem("data",JSON.stringify(data));
    console.log('Protected Data:', data);
}
