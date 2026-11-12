import { useState } from "react";
import "./AuthPage.css";

const API_URL = "https://localhost:7270/api/Auth";

function UserIcon() {
    return (
        <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round" aria-hidden="true">
            <path d="M19 21v-2a4 4 0 0 0-4-4H9a4 4 0 0 0-4 4v2" />
            <circle cx="12" cy="7" r="4" />
        </svg>
    );
}

function MailIcon() {
    return (
        <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round" aria-hidden="true">
            <rect width="20" height="16" x="2" y="4" rx="2" />
            <path d="m22 7-8.97 5.7a1.94 1.94 0 0 1-2.06 0L2 7" />
        </svg>
    );
}

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
        <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round" aria-hidden="true">
            <path d="M3 3l18 18" />
            <path d="M10.6 10.6a2 2 0 002.8 2.8" />
            <path d="M9.9 5.3A10.8 10.8 0 0112 5c5.5 0 9.5 7 9.5 7a15 15 0 01-3.2 4" />
            <path d="M6.2 6.2C3.8 8 2.5 12 2.5 12s4 7 9.5 7a10 10 0 004.2-.9" />
        </svg>
    ) : (
        <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round" aria-hidden="true">
            <path d="M2.5 12s3.5-7 9.5-7 9.5 7 9.5 7-3.5 7-9.5 7-9.5-7-9.5-7z" />
            <circle cx="12" cy="12" r="3" />
        </svg>
    );
}

function AppleIcon() {
    return (
        <svg width="21" height="21" viewBox="0 0 24 24" fill="currentColor" aria-hidden="true">
            <path d="M17.05 12.54c.02 2.14 1.88 2.85 1.9 2.86-.02.05-.3 1.05-.98 2.08-.58.89-1.18 1.77-2.13 1.79-.93.02-1.23-.58-2.29-.58-1.06 0-1.39.56-2.27.6-.92.03-1.62-.97-2.2-1.86-1.2-1.82-2.1-5.13-.88-7.36a3.42 3.42 0 0 1 2.88-1.76c.9-.02 1.75.64 2.3.64.54 0 1.57-.8 2.65-.68.45.02 1.72.18 2.54 1.38-.07.04-1.52.88-1.52 2.89ZM15.33 7.14c.48-.59.81-1.41.72-2.24-.7.03-1.56.47-2.06 1.05-.45.51-.84 1.34-.74 2.14.78.06 1.58-.4 2.08-.95Z" />
        </svg>
    );
}

function GoogleIcon() {
    return (
        <svg width="21" height="21" viewBox="0 0 24 24" aria-hidden="true">
            <path fill="#4285F4" d="M21.6 12.23c0-.7-.06-1.37-.18-2.02H12v3.82h5.38a4.6 4.6 0 0 1-2 3.02v2.5h3.23c1.9-1.74 2.99-4.3 2.99-7.32Z" />
            <path fill="#34A853" d="M12 22c2.7 0 4.97-.9 6.62-2.45l-3.23-2.5c-.9.6-2.04.97-3.39.97-2.61 0-4.83-1.76-5.62-4.13H3.05v2.57A10 10 0 0 0 12 22Z" />
            <path fill="#FBBC05" d="M6.38 13.89A6 6 0 0 1 6.07 12c0-.66.11-1.3.31-1.89V7.54H3.05A10 10 0 0 0 2 12c0 1.61.38 3.12 1.05 4.46l3.33-2.57Z" />
            <path fill="#EA4335" d="M12 5.98c1.47 0 2.79.5 3.83 1.5l2.87-2.87A9.56 9.56 0 0 0 12 2a10 10 0 0 0-8.95 5.54l3.33 2.57C7.17 7.74 9.39 5.98 12 5.98Z" />
        </svg>
    );
}

function FacebookIcon() {
    return (
        <svg width="20" height="20" viewBox="0 0 24 24" fill="currentColor" aria-hidden="true">
            <path d="M22 12a10 10 0 1 0-11.56 9.88v-6.99H7.9V12h2.54V9.8c0-2.51 1.49-3.9 3.78-3.9 1.1 0 2.25.2 2.25.2v2.47H15.2c-1.25 0-1.64.78-1.64 1.58V12h2.79l-.45 2.89h-2.34v6.99A10 10 0 0 0 22 12Z" />
        </svg>
    );
}

function ResetLockIcon() {
    return (
        <svg width="34" height="34" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.7" strokeLinecap="round" strokeLinejoin="round" aria-hidden="true">
            <rect x="4" y="10" width="16" height="11" rx="2" />
            <path d="M8 10V7a4 4 0 0 1 8 0v3" />
            <circle cx="12" cy="15" r="1" />
            <path d="M12 16v2" />
        </svg>
    );
}

const passwordChecks = [
    { label: "En az 8 karakter", test: value => value.length >= 8 },
    { label: "Büyük harf", test: value => /[A-Z]/.test(value) },
    { label: "Küçük harf", test: value => /[a-z]/.test(value) },
    { label: "Rakam", test: value => /[0-9]/.test(value) },
    { label: "Özel karakter", test: value => /[^A-Za-z0-9]/.test(value) }
];

async function postAuth(endpoint, payload) {
    const response = await fetch(`${API_URL}${endpoint}`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload)
    });

    const text = await response.text();
    let data = {};

    if (text) {
        try {
            data = JSON.parse(text);
        } catch {
            data = { message: text };
        }
    }

    return { response, data };
}

function getApiMessage(data) {
    return [
        data?.detail,
        data?.message,
        data?.title
    ].filter(Boolean).join(" ");
}

export default function AuthPage({ onLogin, onClose, initialTab = "login" }) {
    const [tab, setTab] = useState(initialTab);
    const [step, setStep] = useState(1);

    const [showPassword, setShowPassword] = useState(false);
    const [showConfirmPassword, setShowConfirmPassword] = useState(false);
    const [rememberMe, setRememberMe] = useState(false);

    const [loading, setLoading] = useState(false);
    const [resending, setResending] = useState(false);

    const [error, setError] = useState("");
    const [success, setSuccess] = useState("");

    const [verificationEmail, setVerificationEmail] = useState("");

    const [form, setForm] = useState({
        firstName: "",
        lastName: "",
        email: "",
        password: "",
        confirmPassword: ""
    });

    const passedChecks = passwordChecks.filter(item => item.test(form.password)).length;

    function updateField(event) {
        const { name, value } = event.target;
        setForm(previous => ({
            ...previous,
            [name]: value
        }));
    }

    function changeTab(nextTab) {
        setTab(nextTab);
        setStep(1);
        setError("");
        setSuccess("");
        setShowPassword(false);
        setShowConfirmPassword(false);
    }

    function openForgotPassword() {
        changeTab("forgot");
    }

    function openVerification(email) {
        setVerificationEmail(email.trim());
        setTab("verify");
        setStep(1);
        setError("");
        setSuccess("");
    }

    function getSubmitButtonText() {
        if (loading) return "Lütfen bekle...";
        if (tab === "forgot") return success ? "Bağlantı Talep Edildi" : "Sıfırlama Bağlantısı Gönder";
        if (tab === "login") return "Giriş yap";
        return step === 1 ? "Devam et" : "Üyeliği tamamla";
    }

    async function resendConfirmation() {
        if (resending || !verificationEmail) return;

        setResending(true);
        setError("");
        setSuccess("");

        try {
            const { response, data } = await postAuth("/resend-confirmation", { email: verificationEmail });

            if (!response.ok) {
                throw new Error(getApiMessage(data) || "Doğrulama e-postası gönderilemedi.");
            }

            setSuccess("Hesabınız doğrulama bekliyorsa yeni bir bağlantı gönderilecektir.");
        } catch (err) {
            setError(
                err instanceof TypeError
                    ? "Sunucuya bağlanılamadı."
                    : err.message || "Beklenmeyen bir hata oluştu."
            );
        } finally {
            setResending(false);
        }
    }

    async function submit(event) {
        event.preventDefault();
        if (loading) return;

        setError("");
        setSuccess("");

        if (tab === "register" && step === 1) {
            if (!form.email.trim()) {
                setError("E-posta adresini gir.");
                return;
            }
            setStep(2);
            return;
        }

        if (tab === "register" && passedChecks !== passwordChecks.length) {
            setError("Şifreniz tüm güvenlik gereksinimlerini karşılamalı.");
            return;
        }

        if (tab === "register" && form.password !== form.confirmPassword) {
            setError("Şifreler eşleşmiyor.");
            return;
        }

        setLoading(true);

        try {
            let endpoint = "";
            let payload = {};

            if (tab === "forgot") {
                endpoint = "/forgot-password";
                payload = { email: form.email.trim() };
            } else if (tab === "login") {
                endpoint = "/login";
                payload = { email: form.email.trim(), password: form.password, rememberMe };
            } else {
                endpoint = "/register";
                payload = {
                    firstName: form.firstName.trim(),
                    lastName: form.lastName.trim(),
                    email: form.email.trim(),
                    password: form.password,
                    confirmPassword: form.confirmPassword
                };
            }

            const { response, data } = await postAuth(endpoint, payload);

            if (!response.ok) {
                const apiMessage = getApiMessage(data);

                if (response.status === 401) {
                    const requiresVerification = /verify your email|email.*confirm|email.*verif|e-posta.*doğrula/i.test(apiMessage);

                    if (tab === "login" && requiresVerification) {
                        openVerification(form.email);
                        return;
                    }

                    throw new Error("E-posta veya şifre hatalı.");
                }

                if (response.status === 409) {
                    throw new Error(
                        "Bu e-posta adresi zaten kayıtlı. Hesabınız doğrulanmadıysa giriş ekranından doğrulama bağlantısı isteyebilirsiniz."
                    );
                }

                if (response.status === 400) {
                    throw new Error("Bilgilerini kontrol et. Şifre en az 8 karakter, büyük/küçük harf, rakam ve özel karakter içermeli.");
                }

                throw new Error(apiMessage || "İşlem başarısız oldu.");
            }

            if (tab === "forgot") {
                setSuccess("Bu e-posta adresine ait bir hesap varsa şifre sıfırlama bağlantısı gönderilecektir.");
                return;
            }

            if (tab === "register") {
                if (data.requiresEmailConfirmation) {
                    openVerification(form.email);
                    return;
                }

                throw new Error("Kayıt yanıtı beklenen formatta değil.");
            }

            if (!data.accessToken) {
                throw new Error("Giriş yanıtında erişim token'ı bulunamadı.");
            }

            onLogin(data, rememberMe);
        } catch (err) {
            setError(
                err instanceof TypeError
                    ? "Sunucuya bağlanılamadı. API adresini ve HTTPS sertifikasını kontrol et."
                    : err.message || "Beklenmeyen bir hata oluştu."
            );
        } finally {
            setLoading(false);
        }
    }

    return (
        <div className="auth-overlay">
            <section className="auth-panel" role="dialog" aria-modal="true" aria-label="Hesap işlemleri">
                <button type="button" className="auth-close" onClick={onClose} aria-label="Kapat">
                    ×
                </button>

                {tab === "verify" ? (
                    <div className="auth-forgot-header">
                        <div className="auth-forgot-icon">
                            <MailIcon />
                        </div>
                        <h2>E-postanı Doğrula</h2>
                        <p>Üyeliğini tamamlamak için e-posta adresine gönderdiğimiz doğrulama bağlantısına tıkla.</p>
                        <p><strong>{verificationEmail}</strong></p>
                        <p>Bağlantı 24 saat geçerlidir. E-posta gelmediyse spam klasörünü de kontrol edebilirsin.</p>
                    </div>
                ) : tab === "forgot" ? (
                    <div className="auth-forgot-header">
                        <div className="auth-forgot-icon">
                            <ResetLockIcon />
                        </div>
                        <h2>Şifremi Unuttum</h2>
                        <p>Hesabınıza kayıtlı e-posta adresini girin. Size şifrenizi yenilemeniz için bir bağlantı gönderelim.</p>
                    </div>
                ) : (
                    <>
                        <div className="auth-header-top">
                            <div className="auth-brand-logo">B</div>
                            <h2>Hoş Geldiniz</h2>
                            <p>Devam etmek için lütfen giriş yapın veya üye olun.</p>
                        </div>

                        <div className="auth-social-minimal">
                            <button type="button" disabled title="Apple ile giriş yakında" aria-label="Apple ile giriş (yakında)">
                                <AppleIcon />
                            </button>
                            <button type="button" disabled title="Google ile giriş yakında" aria-label="Google ile giriş (yakında)">
                                <GoogleIcon />
                            </button>
                            <button type="button" disabled title="Facebook ile giriş yakında" aria-label="Facebook ile giriş (yakında)">
                                <FacebookIcon />
                            </button>
                        </div>

                        <div className="auth-divider">
                            <span>VEYA</span>
                        </div>

                        <div className="auth-tabs">
                            <button type="button" className={tab === "login" ? "active" : ""} onClick={() => changeTab("login")}>
                                Giriş yap
                            </button>
                            <button type="button" className={tab === "register" ? "active" : ""} onClick={() => changeTab("register")}>
                                Üye ol
                            </button>
                        </div>
                    </>
                )}

                {tab === "verify" ? (
                    <div className="auth-form">
                        {error && <p className="auth-error" role="alert">{error}</p>}
                        {success && <p className="auth-success" role="status">{success}</p>}

                        <button type="button" className="auth-submit" disabled={resending} onClick={resendConfirmation}>
                            {resending ? "Gönderiliyor..." : "Doğrulama E-postasını Tekrar Gönder"}
                        </button>

                        <button type="button" className="auth-back" onClick={() => changeTab("login")}>
                            ← Giriş ekranına dön
                        </button>
                    </div>
                ) : (
                    <form className="auth-form" onSubmit={submit}>
                        {tab === "register" && step === 2 && (
                            <>
                                <div className="auth-input-wrapper">
                                    <span className="auth-input-icon"><UserIcon /></span>
                                    <input
                                        name="firstName"
                                        placeholder="Ad"
                                        value={form.firstName}
                                        onChange={updateField}
                                        maxLength={100}
                                        autoComplete="given-name"
                                        required
                                    />
                                </div>
                                <div className="auth-input-wrapper">
                                    <span className="auth-input-icon"><UserIcon /></span>
                                    <input
                                        name="lastName"
                                        placeholder="Soyad"
                                        value={form.lastName}
                                        onChange={updateField}
                                        maxLength={100}
                                        autoComplete="family-name"
                                        required
                                    />
                                </div>
                            </>
                        )}

                        {(tab === "login" || tab === "forgot" || (tab === "register" && step === 1)) && (
                            <div className="auth-input-wrapper">
                                <span className="auth-input-icon"><MailIcon /></span>
                                <input
                                    name="email"
                                    type="email"
                                    placeholder="E-posta adresi"
                                    value={form.email}
                                    onChange={updateField}
                                    autoComplete="email"
                                    required
                                />
                            </div>
                        )}

                        {(tab === "login" || (tab === "register" && step === 2)) && (
                            <>
                                <div className="auth-input-wrapper">
                                    <span className="auth-input-icon"><LockIcon /></span>
                                    <input
                                        name="password"
                                        type={showPassword ? "text" : "password"}
                                        placeholder="Şifre"
                                        value={form.password}
                                        onChange={updateField}
                                        autoComplete={tab === "login" ? "current-password" : "new-password"}
                                        minLength={tab === "register" ? 8 : undefined}
                                        required
                                    />
                                    <button
                                        type="button"
                                        className="auth-eye-button"
                                        onClick={() => setShowPassword(prev => !prev)}
                                        aria-label={showPassword ? "Şifreyi gizle" : "Şifreyi göster"}
                                        aria-pressed={showPassword}
                                    >
                                        <EyeIcon visible={showPassword} />
                                    </button>
                                </div>

                                {tab === "register" && (
                                    <div className="auth-password-strength" aria-live="polite">
                                        <div className="auth-strength-track">
                                            <div className="auth-strength-fill" style={{ width: `${passedChecks * 20}%` }} />
                                        </div>
                                        <span>
                                            {!form.password
                                                ? "Şifre güvenliği"
                                                : passedChecks <= 2
                                                    ? "Zayıf"
                                                    : passedChecks < 5
                                                        ? "Orta"
                                                        : "Güçlü"}
                                        </span>
                                        <div className="auth-password-checks">
                                            {passwordChecks.map(item => (
                                                <span key={item.label} className={item.test(form.password) ? "met" : ""}>
                                                    {item.test(form.password) ? "✓" : "○"} {item.label}
                                                </span>
                                            ))}
                                        </div>
                                    </div>
                                )}

                                {tab === "register" && (
                                    <div className="auth-input-wrapper">
                                        <span className="auth-input-icon"><LockIcon /></span>
                                        <input
                                            name="confirmPassword"
                                            type={showConfirmPassword ? "text" : "password"}
                                            placeholder="Şifre tekrar"
                                            value={form.confirmPassword}
                                            onChange={updateField}
                                            autoComplete="new-password"
                                            required
                                        />
                                        <button
                                            type="button"
                                            className="auth-eye-button"
                                            onClick={() => setShowConfirmPassword(prev => !prev)}
                                            aria-label={showConfirmPassword ? "Şifreyi gizle" : "Şifreyi göster"}
                                            aria-pressed={showConfirmPassword}
                                        >
                                            <EyeIcon visible={showConfirmPassword} />
                                        </button>
                                    </div>
                                )}

                                {tab === "login" && (
                                    <div className="auth-login-options">
                                        <label className="auth-remember">
                                            <input
                                                type="checkbox"
                                                checked={rememberMe}
                                                onChange={event => setRememberMe(event.target.checked)}
                                            />
                                            <span>Beni hatırla</span>
                                        </label>
                                        <button type="button" className="auth-forgot-link" onClick={openForgotPassword}>
                                            Şifremi Unuttum
                                        </button>
                                    </div>
                                )}
                            </>
                        )}

                        {error && <p className="auth-error" role="alert">{error}</p>}
                        {success && <p className="auth-success" role="status">{success}</p>}

                        <button type="submit" className="auth-submit" disabled={loading || (tab === "forgot" && Boolean(success))}>
                            {getSubmitButtonText()}
                        </button>

                        {tab === "register" && step === 2 && (
                            <button type="button" className="auth-back" onClick={() => setStep(1)}>
                                Geri dön
                            </button>
                        )}

                        {tab === "forgot" && (
                            <button type="button" className="auth-back" onClick={() => changeTab("login")}>
                                ← Giriş ekranına dön
                            </button>
                        )}
                    </form>
                )}
            </section>
        </div>
    );
}