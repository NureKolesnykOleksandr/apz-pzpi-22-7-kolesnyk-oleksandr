package com.example.mobileapllication

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Refresh
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.TextStyle
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.example.mobileapllication.APIServices.DeviceService
import com.example.mobileapllication.Dtos.DeviceDto
import com.example.mobileapllication.Dtos.SensorDataDto
import com.example.mobileapllication.ui.theme.MobileApllicationTheme
import java.util.concurrent.CompletableFuture

@Composable
fun DeviceScreen(
    userId: Int = 1,
    deviceService: DeviceService = DeviceService()
) {
    var devices by remember { mutableStateOf<List<DeviceDto>?>(null) }
    var deviceHistories by remember { mutableStateOf<Map<Int, List<SensorDataDto>>>(emptyMap()) }
    var isLoading by remember { mutableStateOf(true) }
    var errorMessage by remember { mutableStateOf<String?>(null) }
    val loadingDevices = remember { mutableSetOf<Int>() }

    // Функція для завантаження історії пристрою
    fun loadDeviceHistory(deviceId: Int, deviceService: DeviceService) {
        println("Завантажуємо історію для пристрою: $deviceId")

        deviceService.getDeviceHistoryAsync(deviceId)
            .thenAccept { history ->
                if (history != null) {
                    println("Завантажено ${history.size} записів для пристрою $deviceId")
                    deviceHistories = deviceHistories + (deviceId to history)
                } else {
                    println("Не вдалося завантажити історію для пристрою $deviceId")
                }

                loadingDevices.remove(deviceId)
                if (loadingDevices.isEmpty()) {
                    isLoading = false
                    println("Завантаження завершено успішно")
                }
            }
            .exceptionally { error ->
                println("Помилка при завантаженні історії: ${error.message}")
                error.printStackTrace()
                loadingDevices.remove(deviceId)
                if (loadingDevices.isEmpty()) {
                    isLoading = false
                }
                null
            }
    }
    // Функція для завантаження пристроїв
    fun loadDevices() {
        isLoading = true
        errorMessage = null
        println("Починаємо завантаження пристроїв для користувача: $userId")

        deviceService.getDevicesAsync(userId)
            .thenAccept { devicesResult ->
                if (devicesResult != null && devicesResult.isNotEmpty()) {
                    devices = devicesResult
                    println("Завантажено ${devicesResult.size} пристроїв")

                    // Завантажуємо історію для кожного пристрою
                    devicesResult.forEach { device ->
                        if (!loadingDevices.contains(device.deviceId)) {
                            loadingDevices.add(device.deviceId)
                            loadDeviceHistory(device.deviceId, deviceService)
                        }
                    }
                } else {
                    println("Пристрої не знайдено або результат null")
                    errorMessage = "Пристрої не знайдено"
                    isLoading = false
                }
            }
            .exceptionally { error ->
                println("Помилка при завантаженні пристроїв: ${error.message}")
                error.printStackTrace()
                errorMessage = "Помилка завантаження: ${error.message}"
                isLoading = false
                null
            }
    }


    // Завантаження даних при запуску екрану
    LaunchedEffect(userId) {
        loadDevices()
    }

    Column(
        modifier = Modifier
            .fillMaxSize()
            .padding(16.dp)
    ) {
        Text(
            text = "Мої пристрої",
            style = MaterialTheme.typography.headlineMedium.copy(
                color = Color.Black,
                fontWeight = FontWeight.Bold
            ),
            modifier = Modifier.padding(bottom = 16.dp)
        )

        when {
            isLoading -> {
                Box(
                    modifier = Modifier.fillMaxSize(),
                    contentAlignment = Alignment.Center
                ) {
                    Column(
                        horizontalAlignment = Alignment.CenterHorizontally
                    ) {
                        CircularProgressIndicator(
                            color = Color(0xFF4CAF50)
                        )
                        Spacer(modifier = Modifier.height(16.dp))
                        Text(
                            text = "Завантаження пристроїв...",
                            style = MaterialTheme.typography.bodyMedium
                        )
                    }
                }
            }

            errorMessage != null -> {
                Card(
                    modifier = Modifier.fillMaxWidth(),
                    colors = CardDefaults.cardColors(containerColor = Color(0xFFFFEBEE))
                ) {
                    Column(
                        modifier = Modifier.padding(16.dp)
                    ) {
                        Text(
                            text = errorMessage!!,
                            color = Color(0xFFD32F2F),
                            style = MaterialTheme.typography.bodyMedium
                        )

                        Spacer(modifier = Modifier.height(8.dp))

                        Button(
                            onClick = { loadDevices() },
                            colors = ButtonDefaults.buttonColors(
                                containerColor = Color(0xFF4CAF50)
                            )
                        ) {
                            Text("Спробувати знову", color = Color.White)
                        }
                    }
                }
            }

            devices.isNullOrEmpty() -> {
                Box(
                    modifier = Modifier.fillMaxSize(),
                    contentAlignment = Alignment.Center
                ) {
                    Text(
                        text = "Пристроїв не знайдено",
                        style = MaterialTheme.typography.bodyLarge.copy(
                            color = Color.Gray
                        )
                    )
                }
            }

            else -> {
                LazyColumn(
                    verticalArrangement = Arrangement.spacedBy(16.dp)
                ) {
                    items(devices!!) { device ->
                        DeviceCard(
                            device = device,
                            history = deviceHistories[device.deviceId] ?: emptyList(),
                            onRefresh = { loadDeviceHistory(device.deviceId, deviceService) }
                        )
                    }
                }
            }
        }
    }
}

@Composable
fun DeviceCard(
    device: DeviceDto,
    history: List<SensorDataDto>,
    onRefresh: () -> Unit = {}
) {
    Card(
        modifier = Modifier.fillMaxWidth(),
        shape = RoundedCornerShape(12.dp),
        colors = CardDefaults.cardColors(containerColor = Color.White),
        elevation = CardDefaults.cardElevation(defaultElevation = 4.dp)
    ) {
        Column(
            modifier = Modifier.padding(16.dp)
        ) {
            Row(
                modifier = Modifier.fillMaxWidth(),
                horizontalArrangement = Arrangement.SpaceBetween,
                verticalAlignment = Alignment.CenterVertically
            ) {
                Column {
                    Text(
                        text = device.deviceName,
                        style = MaterialTheme.typography.headlineSmall.copy(
                            fontWeight = FontWeight.Bold,
                            color = Color.Black
                        )
                    )
                    Text(
                        text = device.deviceType,
                        style = MaterialTheme.typography.bodyMedium.copy(
                            color = Color.Gray
                        )
                    )
                    Text(
                        text = "S/N: ${device.serialNumber}",
                        style = MaterialTheme.typography.bodySmall.copy(
                            color = Color.Gray
                        )
                    )
                }

                Row(verticalAlignment = Alignment.CenterVertically) {
                    Surface(
                        shape = RoundedCornerShape(20.dp),
                        color = if (history.isNotEmpty()) Color(0xFF4CAF50) else Color(0xFFFF9800)
                    ) {
                        Text(
                            text = if (history.isNotEmpty()) "Активний" else "Немає даних",
                            color = Color.White,
                            fontSize = 12.sp,
                            modifier = Modifier.padding(horizontal = 12.dp, vertical = 4.dp)
                        )
                    }

                    Spacer(modifier = Modifier.width(8.dp))

                    IconButton(
                        onClick = onRefresh,
                        modifier = Modifier.size(24.dp)
                    ) {
                        Icon(
                            imageVector = Icons.Default.Refresh,
                            contentDescription = "Оновити",
                            tint = Color(0xFF4CAF50)
                        )
                    }
                }
            }

            Spacer(modifier = Modifier.height(16.dp))

            Text(
                text = "Історія показників:",
                style = MaterialTheme.typography.titleMedium.copy(
                    fontWeight = FontWeight.SemiBold,
                    color = Color.Black
                ),
                modifier = Modifier.padding(bottom = 8.dp)
            )

            if (history.isNotEmpty()) {
                SensorDataTable(history = history)
            } else {
                Text(
                    text = "Немає даних для відображення",
                    style = MaterialTheme.typography.bodyMedium.copy(
                        color = Color.Gray
                    ),
                    modifier = Modifier.padding(vertical = 8.dp)
                )
            }
        }
    }
}

@Composable
fun SensorDataTable(history: List<SensorDataDto>) {
    Column {
        // Заголовки таблиці
        Row(
            modifier = Modifier
                .fillMaxWidth()
                .background(
                    Color(0xFFF5F5F5),
                    shape = RoundedCornerShape(topStart = 8.dp, topEnd = 8.dp)
                )
                .padding(8.dp),
            horizontalArrangement = Arrangement.SpaceBetween
        ) {
            TableHeader("Час", 0.2f)
            TableHeader("Пульс", 0.15f)
            TableHeader("SpO2", 0.15f)
            TableHeader("Темп.", 0.15f)
            TableHeader("Активність", 0.2f)
            TableHeader("Сон", 0.13f)
        }

        // Рядки даних (показуємо останні 5 записів)
        history.takeLast(5).forEach { data ->
            Row(
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(8.dp),
                horizontalArrangement = Arrangement.SpaceBetween
            ) {
                TableCell(formatTimestamp(data.timestamp), 0.2f)
                TableCell("${data.heartRate}", 0.15f)
                TableCell("${data.bloodOxygenLevel}%", 0.15f)
                TableCell("${data.bodyTemperature}°C", 0.15f)
                TableCell(data.activityLevel, 0.2f)
                TableCell(data.sleepPhase, 0.13f)
            }

            Divider(color = Color(0xFFE0E0E0), thickness = 0.5.dp)
        }

        // Показати кількість записів
        if (history.size > 5) {
            Text(
                text = "Показано 5 з ${history.size} записів",
                style = MaterialTheme.typography.bodySmall.copy(
                    color = Color.Gray
                ),
                modifier = Modifier.padding(8.dp)
            )
        }
    }
}

@Composable
fun TableHeader(text: String, weight: Float) {
    Box(
        modifier = Modifier.fillMaxWidth(weight),
        contentAlignment = Alignment.Center
    ) {
        Text(
            text = text,
            style = MaterialTheme.typography.bodySmall.copy(
                fontWeight = FontWeight.Bold,
                color = Color.Black
            )
        )
    }
}

@Composable
fun TableCell(text: String, weight: Float) {
    Box(
        modifier = Modifier.fillMaxWidth(weight),
        contentAlignment = Alignment.Center
    ) {
        Text(
            text = text,
            style = MaterialTheme.typography.bodySmall.copy(
                color = Color.Black
            )
        )
    }
}

// Функція для форматування часу
fun formatTimestamp(timestamp: String): String {
    return try {
        val parts = timestamp.split("T")
        if (parts.size > 1) {
            val timePart = parts[1].split(".")[0]
            timePart.substring(0, 5)
        } else {
            timestamp.take(10)
        }
    } catch (e: Exception) {
        timestamp.take(8)
    }
}

@Preview(showBackground = true)
@Composable
fun DeviceScreenPreview() {
    MobileApllicationTheme {
        Surface(
            modifier = Modifier.fillMaxSize(),
            color = Color(0xFFF5F5F5)
        ) {
            Column(modifier = Modifier.fillMaxSize()) {
                Box(
                    modifier = Modifier
                        .fillMaxWidth()
                        .height(64.dp)
                        .background(Color(0xFF4CAF50))
                ) {
                    Text(
                        text = "MedMon",
                        color = Color.White,
                        style = TextStyle(
                            fontSize = 20.sp,
                            fontWeight = FontWeight.Bold
                        ),
                        modifier = Modifier.align(Alignment.Center)
                    )
                }
                DeviceScreen()
            }
        }
    }
}