function AddBalance() {
    const url = "http://localhost:5179/api/Account/UpdateBalance?id=1&value=500";
    // localStorage'dan access token'ı al
    const token = localStorage.getItem("accessToken");

    if (!token) {
console.error("No access token found");
        return;
    }

    try {
    const response = fetch(url, { // Değiştirin
            method: "POST", // Veya ihtiyaca göre başka bir method (POST, PUT, vb.)
            headers: {
                "Authorization": `Bearer ${token}`, // Token'ı Authorization header'ında gönder
            "Content-Type": "application/json"
            }
        });

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

    const result = response.json();
        console.log("Protected resource data:", result);
    } catch (error) {
        console.error("Error fetching protected resource:", error);
    }
}