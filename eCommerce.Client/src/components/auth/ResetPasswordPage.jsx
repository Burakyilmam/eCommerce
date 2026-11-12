import { useEffect, useState } from "react";
import Swal from "sweetalert2";

import "./ResetPasswordPage.css";

const API_URL = "https://localhost:7270";

function LockIcon() {
    return (
        <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round" aria-hidden="true">
            <rect width="16" height="11" x="4" y="11" rx="2" ry="2" />
            <path d="M7 11V7a5 5 0 0 1 10 0v4" />
        </svg>
    );
}

function EyeIcon({ visible }) {
    return visible ? (
        <svg width="21" height="21" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round" aria-hidden="true">
            <path d="M3 3l18 18" />
            <path d="M10.6 10.6a2 2 0 002.8 2.8" />
            <path d="M9.9 5.3A10.8 10.8 0 0112 5c5.5 0 9.5 7 9.5 7a15 15 0 01-3.2 4" />
            <path d="M6.2 6.2C3.8 8 2.5 12 2.5 12s4 7 9.5 7a10 10 0 004.2-.9" />
        </svg>
    ) : (
        <svg width="21" height="21" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round" aria-hidden="true">
            <path d="M2.5 12s3.5-7 9.5-7 9.5 7 9.5 7-3.5 7-9.5 7-9.5-7-9.5-7z" />
            <circle cx="12" cy="12" r="3" />
        </svg>
    );
}

function Requirement({ valid, text }) {
    return (
        <div className={`reset-requirement ${valid ? "valid" : ""}`}>
            <span aria-hidden="true">{valid ? "✓" : "○"}</span>
            <span>{text}</span>
        </div>
    );
}

export default function ResetPasswordPage() {
    const params = new URLSearchParams(window.location.search);

    const userId = params.get("userId");
    const token = params.get("token");

    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");

    const [showPassword, setShowPassword] = useState(false);
    const [showConfirmPassword, setShowConfirmPassword] = useState(false);

    const [loading, setLoading] = useState(false);
    const [success, setSuccess] = useState(false);
    const [linkStatus, setLinkStatus] = useState("checking");
    const [retryCount, setRetryCount] = useState(0);

    const isValidLink = Boolean(userId && token);

    useEffect(() => {
        if (!isValidLink) {
            setLinkStatus("invalid");
            return;
        }

        const controller = new AbortController();
        setLinkStatus("checking");

        async function validateLink() {
            try {
                const response = await fetch(`${API_URL}/api/Auth/validate-reset-token`, {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({ userId, token }),
                    signal: controller.signal
                });

                if (!response.ok) {
                    throw new Error("Token doğrulama servisine ulaşılamadı.");
                }

                const data = await response.json();
                if (!controller.signal.aborted) {
                    setLinkStatus(data.isValid === true ? "valid" : "invalid");
                }
            } catch (error) {
                if (!controller.signal.aborted) {
                    setLinkStatus("error");
                }
            }
        }

        validateLink();
        return () => controller.abort();
    }, [userId, token, isValidLink, retryCount]);

    const passwordChecks = {
        length: password.length >= 8,
        uppercase: /[A-Z]/.test(password),
        lowercase: /[a-z]/.test(password),
        number: /[0-9]/.test(password),
        special: /[^A-Za-z0-9]/.test(password)
    };

    const passwordScore = Object.values(passwordChecks).filter(Boolean).length;
    const passwordValid = passwordScore === 5;
    const passwordsMatch = confirmPassword.length > 0 && password === confirmPassword;

    const canSubmit = linkStatus === "valid" && passwordValid && passwordsMatch && !loading;

    const passwordStrength =
        password.length === 0
            ? { label: "", level: 0 }
            : passwordScore <= 2
                ? { label: "Zayıf", level: 1 }
                : passwordScore <= 4
                    ? { label: "Orta", level: 2 }
                    : { label: "Güçlü", level: 3 };

    const strengthWidth =
        passwordStrength.level === 1
            ? "33%"
            : passwordStrength.level === 2
                ? "67%"
                : passwordStrength.level === 3
                    ? "100%"
                    : "0%";

    const strengthClass =
        passwordStrength.level === 1
            ? "weak"
            : passwordStrength.level === 2
                ? "medium"
                : passwordStrength.level === 3
                    ? "strong"
                    : "";

    async function handleSubmit(event) {
        event.preventDefault();

        if (!canSubmit) return;

        setLoading(true);

        try {
            const response = await fetch(`${API_URL}/api/Auth/reset-password`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    userId,
                    token,
                    newPassword: password,
                    confirmPassword
                })
            });

            if (!response.ok) {
                if (response.status === 400) {
                    try {
                        const check = await fetch(`${API_URL}/api/Auth/validate-reset-token`, {
                            method: "POST",
                            headers: { "Content-Type": "application/json" },
                            body: JSON.stringify({ userId, token })
                        });
                        if (check.ok) {
                            const data = await check.json();
                            if (data.isValid === false) {
                                setLinkStatus("invalid");
                                throw new Error("Şifre sıfırlama bağlantınızın süresi dolmuş veya bağlantı geçersiz.");
                            }
                        }
                    } catch (checkError) {
                        if (checkError.message.includes("süresi dolmuş")) throw checkError;
                    }
                }
                throw new Error("Şifre güncellenemedi. Şifre kurallarını kontrol edip tekrar deneyin.");
            }

            setSuccess(true);
            window.history.replaceState({}, "", "/reset-password");

            await Swal.fire({
                icon: "success",
                title: "Şifreniz değiştirildi!",
                text: "Artık yeni şifrenizle giriş yapabilirsiniz.",
                confirmButtonColor: "#f27a1a"
            });

            window.location.replace("/");
        } catch (error) {
            await Swal.fire({
                icon: "error",
                title: "İşlem başarısız",
                text: error.message,
                confirmButtonColor: "#f27a1a"
            });
        } finally {
            setLoading(false);
        }
    }

    if (!success && linkStatus !== "valid") {
        const checking = linkStatus === "checking";
        const error = linkStatus === "error";

        return (
            <main className="reset-page">
                <section className="reset-card" role="status" aria-live="polite">
                    <div className="reset-header">
                        <div className="reset-lock-icon" aria-hidden="true">
                            {checking ? "🔐" : error ? "⚠️" : "⌛"}
                        </div>
                        <h1>
                            {checking
                                ? "Bağlantı Kontrol Ediliyor"
                                : error
                                    ? "Bağlantı Kontrol Edilemedi"
                                    : "Bağlantı Geçersiz veya Süresi Dolmuş"}
                        </h1>
                        <p className="reset-description">
                            {checking
                                ? "Şifre sıfırlama bağlantınız doğrulanıyor, lütfen bekleyin."
                                : error
                                    ? "Sunucuyla bağlantı kurulamadı. Lütfen tekrar deneyin."
                                    : "Şifre sıfırlama bağlantınız artık geçerli değil. Ana sayfadan yeni bir şifre sıfırlama bağlantısı isteyebilirsiniz."}
                        </p>
                    </div>
                    {error && (
                        <button
                            type="button"
                            className="reset-submit-button"
                            onClick={() => setRetryCount(value => value + 1)}
                        >
                            Tekrar Dene
                        </button>
                    )}
                    {!checking && (
                        <a className="reset-home-link" href="/">
                            Ana Sayfaya Dön
                        </a>
                    )}
                </section>
            </main>
        );
    }

    return (
        <main className="reset-page">
            <section className="reset-card">
                <div className="reset-header">
                    <div className="reset-lock-icon" aria-hidden="true">
                        🔐
                    </div>
                    <h1>Yeni Şifre Oluştur</h1>
                    <p className="reset-description">
                        Hesabınız için güvenli bir şifre belirleyin.
                    </p>
                </div>

                <form className="reset-form" onSubmit={handleSubmit}>
                    <div className="reset-field">
                        <label htmlFor="new-password">Yeni Şifre</label>
                        <div className="reset-password-wrapper">
                            <span className="reset-input-icon">
                                <LockIcon />
                            </span>
                            <input
                                id="new-password"
                                type={showPassword ? "text" : "password"}
                                autoComplete="new-password"
                                placeholder="Yeni şifrenizi girin"
                                value={password}
                                onChange={(event) => setPassword(event.target.value)}
                                required
                            />
                            <button
                                type="button"
                                className="reset-eye-button"
                                onClick={() => setShowPassword(value => !value)}
                                aria-label={showPassword ? "Şifreyi gizle" : "Şifreyi göster"}
                                aria-pressed={showPassword}
                            >
                                <EyeIcon visible={showPassword} />
                            </button>
                        </div>
                    </div>

                    <div className="reset-strength">
                        <div
                            className="reset-strength-track"
                            role="progressbar"
                            aria-label="Şifre kurallarını karşılama düzeyi"
                            aria-valuemin={0}
                            aria-valuemax={5}
                            aria-valuenow={passwordScore}
                        >
                            <div
                                className={`reset-strength-fill ${strengthClass}`}
                                style={{ width: strengthWidth }}
                            />
                        </div>
                        <div className={`reset-strength-label ${strengthClass}`} aria-live="polite">
                            {passwordStrength.label}
                        </div>
                    </div>

                    <div className="reset-requirements">
                        <p className="reset-requirements-title">Şifre gereksinimleri</p>
                        <div className="reset-requirements-grid">
                            <Requirement valid={passwordChecks.length} text="En az 8 karakter" />
                            <Requirement valid={passwordChecks.uppercase} text="Büyük harf" />
                            <Requirement valid={passwordChecks.lowercase} text="Küçük harf" />
                            <Requirement valid={passwordChecks.number} text="Rakam" />
                            <Requirement valid={passwordChecks.special} text="Özel karakter" />
                        </div>
                    </div>

                    <div className="reset-field">
                        <label htmlFor="confirm-password">Yeni Şifre Tekrar</label>
                        <div className="reset-password-wrapper">
                            <span className="reset-input-icon">
                                <LockIcon />
                            </span>
                            <input
                                id="confirm-password"
                                type={showConfirmPassword ? "text" : "password"}
                                autoComplete="new-password"
                                placeholder="Yeni şifrenizi tekrar girin"
                                value={confirmPassword}
                                onChange={(event) => setConfirmPassword(event.target.value)}
                                required
                                aria-invalid={confirmPassword.length > 0 && !passwordsMatch}
                            />
                            <button
                                type="button"
                                className="reset-eye-button"
                                onClick={() => setShowConfirmPassword(value => !value)}
                                aria-label={showConfirmPassword ? "Şifreyi gizle" : "Şifreyi göster"}
                                aria-pressed={showConfirmPassword}
                            >
                                <EyeIcon visible={showConfirmPassword} />
                            </button>
                        </div>

                        {confirmPassword.length > 0 && (
                            <p
                                className={`reset-match-message ${passwordsMatch ? "match" : "no-match"}`}
                                role="status"
                            >
                                {passwordsMatch ? "✓ Şifreler eşleşiyor" : "✕ Şifreler eşleşmiyor"}
                            </p>
                        )}
                    </div>

                    <button
                        type="submit"
                        className="reset-submit-button"
                        disabled={!canSubmit}
                    >
                        {loading ? "Şifre güncelleniyor..." : "Şifremi Güncelle"}
                    </button>

                    <a href="/" className="reset-back-link">
                        ← Ana sayfaya dön
                    </a>
                </form>
            </section>
        </main>
    );
}