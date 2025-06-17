package com.example.mobileapllication.APIServices;

import com.example.mobileapllication.Dtos.DeviceDto;
import com.example.mobileapllication.Dtos.SensorDataDto;
import com.fasterxml.jackson.databind.ObjectMapper;
import okhttp3.MediaType;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.Response;

import java.io.IOException;
import java.security.cert.CertificateException;
import java.util.Arrays;
import java.util.List;
import java.util.concurrent.CompletableFuture;
import java.util.concurrent.TimeUnit;

import javax.net.ssl.SSLContext;
import javax.net.ssl.SSLSocketFactory;
import javax.net.ssl.TrustManager;
import javax.net.ssl.X509TrustManager;

public class DeviceService {
    private final OkHttpClient httpClient;
    private final ObjectMapper objectMapper;

    // MediaType для JSON
    private static final MediaType JSON = MediaType.get("application/json; charset=utf-8");
    private static final String BASE_URL = "http://192.168.88.68:2025/api/Device"; // Змінено на HTTP

    public DeviceService() {
        this.objectMapper = new ObjectMapper();
        this.httpClient = getUnsafeOkHttpClient(); // Використовуємо "небезпечний" клієнт для тестування
    }

    public CompletableFuture<List<DeviceDto>> getDevicesAsync(int userId) {
        return CompletableFuture.supplyAsync(() -> {
            try {
                System.out.println("=== Отримання пристроїв для користувача: " + userId + " ===");

                // Створення HTTP-запиту
                Request request = new Request.Builder()
                        .url(BASE_URL + "/" + userId)
                        .get()
                        .addHeader("Content-Type", "application/json")
                        .build();

                System.out.println("URL запиту: " + request.url());

                // Відправка запиту
                try (Response response = httpClient.newCall(request).execute()) {
                    System.out.println("Код відповіді: " + response.code());

                    // Перевірка статусу відповіді
                    if (!response.isSuccessful()) {
                        String errorBody = "";
                        try {
                            errorBody = response.body().string();
                            System.out.println("Тіло помилки: " + errorBody);
                        } catch (Exception e) {
                            System.out.println("Не вдалося прочитати тіло помилки: " + e.getMessage());
                        }
                        System.out.println("Помилка HTTP: " + response.code() + " - " + response.message());
                        return null;
                    }

                    // Отримання тіла відповіді
                    String responseBody = response.body().string();
                    System.out.println("Успішна відповідь: " + responseBody);

                    // Десериалізація відповіді в масив та перетворення в список
                    DeviceDto[] devicesArray = objectMapper.readValue(responseBody, DeviceDto[].class);

                    if (devicesArray == null) {
                        System.out.println("Масив пристроїв null");
                        return null;
                    }

                    List<DeviceDto> devices = Arrays.asList(devicesArray);
                    System.out.println("Отримано пристроїв: " + devices.size());
                    return devices;
                }

            } catch (IOException e) {
                System.out.println("IOException при отриманні пристроїв: " + e.getMessage());
                e.printStackTrace();
                return null;
            } catch (Exception e) {
                System.out.println("Загальна помилка при отриманні пристроїв: " + e.getMessage());
                e.printStackTrace();
                return null;
            }
        });
    }

    public CompletableFuture<List<SensorDataDto>> getDeviceHistoryAsync(int deviceId) {
        return CompletableFuture.supplyAsync(() -> {
            try {
                System.out.println("=== Отримання історії для пристрою: " + deviceId + " ===");

                // Створення HTTP-запиту
                Request request = new Request.Builder()
                        .url(BASE_URL + "/" + deviceId + "/history")
                        .get()
                        .addHeader("Content-Type", "application/json")
                        .build();

                System.out.println("URL запиту: " + request.url());

                // Відправка запиту
                try (Response response = httpClient.newCall(request).execute()) {
                    System.out.println("Код відповіді: " + response.code());

                    // Перевірка статусу відповіді
                    if (!response.isSuccessful()) {
                        String errorBody = "";
                        try {
                            errorBody = response.body().string();
                            System.out.println("Тіло помилки: " + errorBody);
                        } catch (Exception e) {
                            System.out.println("Не вдалося прочитати тіло помилки: " + e.getMessage());
                        }
                        System.out.println("Помилка HTTP: " + response.code() + " - " + response.message());
                        return null;
                    }

                    // Отримання тіла відповіді
                    String responseBody = response.body().string();
                    System.out.println("Успішна відповідь: " + responseBody);

                    // Десериалізація відповіді в масив та перетворення в список
                    SensorDataDto[] sensorDataArray = objectMapper.readValue(responseBody, SensorDataDto[].class);

                    if (sensorDataArray == null) {
                        System.out.println("Масив даних сенсорів null");
                        return null;
                    }

                    List<SensorDataDto> sensorData = Arrays.asList(sensorDataArray);
                    System.out.println("Отримано записів історії: " + sensorData.size());
                    return sensorData;
                }

            } catch (IOException e) {
                System.out.println("IOException при отриманні історії: " + e.getMessage());
                e.printStackTrace();
                return null;
            } catch (Exception e) {
                System.out.println("Загальна помилка при отриманні історії: " + e.getMessage());
                e.printStackTrace();
                return null;
            }
        });
    }

    /**
     * Метод для створення OkHttpClient, який ігнорує SSL помилки (ТІЛЬКИ ДЛЯ ТЕСТУВАННЯ!)
     */
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