package com.example.mobileapllication.APIServices;

import java.io.IOException;
import java.security.cert.CertificateException;
import java.util.concurrent.CompletableFuture;
import java.util.concurrent.TimeUnit;

import javax.net.ssl.SSLContext;
import javax.net.ssl.SSLSocketFactory;
import javax.net.ssl.TrustManager;
import javax.net.ssl.X509TrustManager;

import okhttp3.MediaType;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;
import okhttp3.Response;

import com.example.mobileapllication.Dtos.LoginDto;
import com.example.mobileapllication.Dtos.LoginResultDto;
import com.fasterxml.jackson.databind.ObjectMapper;

public class LoginService {
    private final OkHttpClient httpClient;
    private final ObjectMapper objectMapper;
    private static final MediaType JSON = MediaType.get("application/json; charset=utf-8");

    public LoginService() {
        this.objectMapper = new ObjectMapper();
        this.httpClient = getUnsafeOkHttpClient(); // Використовуємо "небезпечний" клієнт для тестування
    }

    public CompletableFuture<LoginResultDto> loginAsync(LoginDto loginDto) {
        return CompletableFuture.supplyAsync(() -> {
            try {
                String jsonContent = objectMapper.writeValueAsString(loginDto);
                RequestBody body = RequestBody.create(jsonContent, JSON);

                // Основний POST запит
                Request request = new Request.Builder()
                        .url("http://192.168.88.68:2025/api/Auth/login")
                        .post(body)
                        .addHeader("Content-Type", "application/json")
                        .build();

                try (Response response = httpClient.newCall(request).execute()) {
                    if (!response.isSuccessful()) {
                        System.out.println("Помилка: " + response.code());
                        return null;
                    }
                    return objectMapper.readValue(response.body().string(), LoginResultDto.class);
                }
            } catch (Exception e) {
                e.printStackTrace();
                return null;
            }
        });
    }

    // Метод для створення OkHttpClient, який ігнорує SSL помилки (ТІЛЬКИ ДЛЯ ТЕСТУВАННЯ!)
    private static OkHttpClient getUnsafeOkHttpClient() {
        try {
            // Створюємо TrustManager, який ігнорує всі SSL перевірки
            final TrustManager[] trustAllCerts = new TrustManager[]{
                    new X509TrustManager() {
                        @Override
                        public void checkClientTrusted(java.security.cert.X509Certificate[] chain, String authType) throws CertificateException {}
                        @Override
                        public void checkServerTrusted(java.security.cert.X509Certificate[] chain, String authType) throws CertificateException {}
                        @Override
                        public java.security.cert.X509Certificate[] getAcceptedIssuers() {
                            return new java.security.cert.X509Certificate[]{};
                        }
                    }
            };

            // Створюємо SSL контекст
            final SSLContext sslContext = SSLContext.getInstance("SSL");
            sslContext.init(null, trustAllCerts, new java.security.SecureRandom());
            final SSLSocketFactory sslSocketFactory = sslContext.getSocketFactory();

            return new OkHttpClient.Builder()
                    .sslSocketFactory(sslSocketFactory, (X509TrustManager)trustAllCerts[0])
                    .hostnameVerifier((hostname, session) -> true) // Ігноруємо перевірку hostname
                    .connectTimeout(30, TimeUnit.SECONDS)
                    .readTimeout(30, TimeUnit.SECONDS)
                    .writeTimeout(30, TimeUnit.SECONDS)
                    .build();
        } catch (Exception e) {
            throw new RuntimeException(e);
        }
    }
}