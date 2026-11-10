
const API_URL = import.meta.env.VITE_API_URL;

export async function getSliders(signal) {
    if (!API_URL) {
        throw new Error("VITE_API_URL tanımlanmamış.");
    }

    const response = await fetch(`${API_URL}/api/Slider`, {
        signal,
        headers: {
            Accept: "application/json"
        }
    });

    if (!response.ok) {
        throw new Error(
            `Slider verileri alınamadı: HTTP ${response.status}`
        );
    }

    const data = await response.json();

    if (!Array.isArray(data)) {
        throw new Error("API beklenen slider listesini döndürmedi.");
    }

    return data;
}
