package com.example.mobileapllication

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import androidx.compose.material3.*
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.TextStyle
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.example.mobileapllication.APIServices.LoginService
import com.example.mobileapllication.Dtos.LoginDto
import com.example.mobileapllication.ui.theme.MobileApllicationTheme
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch

class MainActivity : ComponentActivity() {
    private var userId: Int = 0

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            MobileApllicationTheme {
                var userId by remember { mutableStateOf(0) }
                var isLoading by remember { mutableStateOf(false) }

                Surface(
                    modifier = Modifier.fillMaxSize(),
                    color = Color(0xFFF5F5F5) // Светло-серый фон для всего экрана
                ) {
                    Column(modifier = Modifier.fillMaxSize()) {
                        // Зеленая верхняя панель
                        Box(
                            modifier = Modifier
                                .fillMaxWidth()
                                .height(64.dp)
                                .background(Color(0xFF4CAF50)) // Красивый зеленый
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
                        // Логика навигации: если userId == 0, показываем LoginScreen, иначе DeviceScreen
                        if (userId == 0) {
                            LoginScreen(
                                onLoginClick = { login, password ->
                                    isLoading = true
                                    // Запрос на авторизацию
                                    CoroutineScope(Dispatchers.Main).launch {
                                        val loginService = LoginService()
                                        val loginDto = LoginDto()
                                        loginDto.email = login
                                        loginDto.password = password

                                        loginService.loginAsync(loginDto)
                                            .thenAccept { result ->
                                                if (result != null) {
                                                    println("Успешная авторизация: userId = ${result.userId}")
                                                    userId = result.userId // Устанавливаем userId, что приведет к переходу на DeviceScreen
                                                } else {
                                                    println("Ошибка авторизации: результат null")
                                                }
                                                isLoading = false
                                            }
                                            .exceptionally { error ->
                                                println("Ошибка при авторизации: ${error.message}")
                                                error.printStackTrace()
                                                isLoading = false
                                                null
                                            }
                                    }
                                },
                                isLoading = isLoading
                            )
                        } else {
                            // Показываем экран устройств для авторизованного пользователя
                            DeviceScreen(
                                userId = userId
                            )
                        }
                    }
                }
            }
        }
    }
}