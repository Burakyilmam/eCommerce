
const API_URL = import.meta.env.VITE_API_URL;

export async function getProducts(signal) {
    if (!API_URL) {
        throw new Error("VITE_API_URL tanımlanmamış.");
    }

    const response = await fetch(`${API_URL}/api/Product`, {
        signal,
        headers: {
            Accept: "application/json"
        }
    });

    if (!response.ok) {
        throw new Error(`Ürünler alınamadı: HTTP ${response.status}`);
    }

    const data = await response.json();

    if (!Array.isArray(data)) {
        throw new Error("API beklenen ürün listesini döndürmedi.");
    }

    return data;
}
