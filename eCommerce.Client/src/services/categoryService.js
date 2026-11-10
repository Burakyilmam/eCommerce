
const API_URL = import.meta.env.VITE_API_URL;

export async function getCategories(signal) {
    const response = await fetch(`${API_URL}/api/Category`, {
        method: 'GET',
        signal,
        headers: {
            'Accept': 'application/json'
        }
    });

    if (!response.ok) {
        throw new Error(
            `Kategoriler alınamadı. HTTP: ${response.status}`
        );
    }

    return await response.json();
}
