const form = document.getElementById("registerform");

form.addEventListener("submit", async (event) => {
    event.preventDefault(); // Formun varsayılan gönderimini engelle

    // Form verilerini al
    const formData = new FormData(form);
    const data = Object.fromEntries(formData.entries());
    // TODO: Form alanlarını kontrol et.
    try {
        const response = await fetch("http://localhost:5179/api/Auth/register", {
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
        console.log("Registration successful:", result);
        // Başarılı kayıt sonrası yapılacak işlemler
    } catch (error) {
        console.error("Error during registration:", error);
        // Hata durumunda yapılacak işlemler
    }
});