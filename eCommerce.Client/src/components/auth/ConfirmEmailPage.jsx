import { useEffect, useRef, useState } from "react";
import "./ConfirmEmailPage.css";

const API_URL = "https://localhost:7270/api/Auth";

function MailCheckIcon() {
    return (
        <svg width="42" height="42" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.6" strokeLinecap="round" strokeLinejoin="round">
            <rect x="2" y="4" width="20" height="16" rx="2" />
            <path d="m22 7-10 6L2 7" />
            <path d="m8 16 2 2 4-4" />
        </svg>
    );
}

function SuccessIcon() {
    return (
        <svg width="64" height="64" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round">
            <circle cx="12" cy="12" r="10" />
            <path d="m7 12 3.5 3.5L17 9" />
        </svg>
    );
}

function ErrorIcon() {
    return (
        <svg width="64" height="64" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round">
            <circle cx="12" cy="12" r="10" />
            <path d="M12 7v6" />
            <path d="M12 17h.01" />
        </svg>
    );
}

function LoadingSpinner() {
    return (
        <div className="confirm-email-spinner" role="status" aria-label="Doğrulanıyor" />
    );
}

export default function ConfirmEmailPage() {
    const [status, setStatus] = useState("loading");
    const [message, setMessage] = useState("");
    const requestStarted = useRef(false);

    useEffect(() => {
        if (requestStarted.current) return;
        requestStarted.current = true;

        const params = new URLSearchParams(window.location.search);
        const userId = params.get("userId");
        const token = params.get("token");

        if (!userId || !token) {
            setStatus("error");
            setMessage("Doğrulama bağlantısı eksik veya geçersiz.");
            return;
        }

        const requestKey = `email-confirmation:${userId}:${token}`;

        if (sessionStorage.getItem(requestKey) === "success") {
            setStatus("success");
            setMessage("E-posta adresiniz başarıyla doğrulandı.");
            return;
        }

        async function confirmEmail() {
            try {
                const response = await fetch(`${API_URL}/confirm-email`, {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({ userId, token })
                });

                let data = {};
                try {
                    data = await response.json();
                } catch {
                    // JSON parse hatası yutulur
                }

                if (!response.ok) {
                    throw new Error(data.message || data.detail || "Bağlantı geçersiz veya süresi dolmuş.");
                }

                sessionStorage.setItem(requestKey, "success");
                setStatus("success");
                setMessage("E-posta adresiniz başarıyla doğrulandı.");
            } catch (error) {
                setStatus("error");
                setMessage(
                    error instanceof TypeError
                        ? "Sunucuya bağlanılamadı. Lütfen daha sonra tekrar deneyin."
                        : error.message || "E-posta doğrulanırken bir hata oluştu."
                );
            }
        }

        confirmEmail();
    }, []);

    function goHome() {
        window.location.href = "/";
    }

    return (
        <div className="confirm-email-page">
            <div className="confirm-email-card">
                <div className="confirm-email-brand">
                    <div className="confirm-email-logo">B</div>
                    <span>HepsiBende</span>
                </div>

                {status === "loading" && (
                    <div className="confirm-email-content">
                        <div className="confirm-email-icon loading">
                            <LoadingSpinner />
                        </div>
                        <h1>E-postanız Doğrulanıyor</h1>
                        <p>Hesabınızı etkinleştiriyoruz. Lütfen kısa bir süre bekleyin.</p>
                    </div>
                )}

                {status === "success" && (
                    <div className="confirm-email-content">
                        <div className="confirm-email-icon success">
                            <SuccessIcon />
                        </div>
                        <h1>E-posta Doğrulandı!</h1>
                        <p>{message}</p>
                        <p className="confirm-email-description">
                            HepsiBende hesabınız artık aktif. Giriş yaparak alışverişe başlayabilirsiniz.
                        </p>
                        <button type="button" className="confirm-email-button" onClick={goHome}>
                            Alışverişe Başla
                        </button>
                    </div>
                )}

                {status === "error" && (
                    <div className="confirm-email-content">
                        <div className="confirm-email-icon error">
                            <ErrorIcon />
                        </div>
                        <h1>Doğrulama Başarısız</h1>
                        <p>{message}</p>
                        <p className="confirm-email-description">
                            Bağlantınızın süresi dolmuş olabilir. Giriş ekranından yeni bir doğrulama e-postası talep edebilirsiniz.
                        </p>
                        <button type="button" className="confirm-email-button" onClick={goHome}>
                            Ana Sayfaya Dön
                        </button>
                    </div>
                )}

                <div className="confirm-email-footer">
                    <MailCheckIcon />
                    <span>Güvenli hesap doğrulama</span>
                </div>
            </div>
        </div>
    );
}