const form = document.getElementById("loginform");

form.addEventListener("submit", async (event) => {
    event.preventDefault(); // Formun varsayılan gönderimini engelle

    // Form verilerini al
    const formData = new FormData(form);
    const data = Object.fromEntries(formData.entries());

    try {
        const response = await fetch("http://localhost:5179/api/Auth/login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        });

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        const result = await response.json();
        console.log("Login successful:", result);

        // Access token'ı localStorage'a kaydet
        localStorage.setItem("accessToken", result.token);

    } catch (error) {
        console.error("Error during login:", error);
        // Hata durumunda yapılacak işlemler
    }
});