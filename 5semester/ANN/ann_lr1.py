# -*- coding: utf-8 -*-
"""ANN LR1.ipynb"""

import numpy as np
import matplotlib.pyplot as plt
import seaborn as

# ============================================
# ЗАДАНИЕ 1: Нейрон МакКаллока-Питтса
# ============================================

def McCuloh_Pitts_neuron(x=None, w=None, bias=0):
    """x - входные сигналы, w - множители, bias - смещение"""
    s = np.dot(w, x.T) + bias
    y = np.sign(s)
    return y

def linear_neuron(x=None, w=None, bias=0):
    s = np.dot(w, x.T) + bias
    y = s
    return y

def sigmoid_neuron(x=None, w=None, bias=0):
    s = np.dot(w, x.T) + bias
    y = 1 / (1 + np.exp(-s))
    return y

def sigmoidt_neuron(x=None, w=None, bias=0):
    s = np.dot(w, x.T) + bias
    y = 2 / (1 + np.exp(-s)) - 1
    return y

def sigmoidt_complex_neuron(x=None, w=None, bias=0, lymb=1):
    s = np.dot(w, x.T) + bias
    y = 2 / (1 + np.exp(-s / lymb)) - 1
    return y

# ============================================
# ЗАДАНИЕ 1: Тестирование нейронов
# ============================================

print("="*60)
print("ЗАДАНИЕ 1: Тестирование нейронов")
print("="*60)

# Тест МакКаллока-Питтса
x = np.array([1, 0])
w = np.array([1, -1])
y_out = McCuloh_Pitts_neuron(x=x, w=w, bias=0.3)
print(f'Ответ нейрона при X=[1,0], W=[1,-1]: {y_out}')

x = np.random.random((10, 2))
w = np.array([1, -1])
y_out1 = McCuloh_Pitts_neuron(x=x, w=w, bias=0.3)
print(f'Ответ нейрона на матрице: {y_out1}')
print('=> нейрон разделяет плоскость прямой линией, X2 = X1 + 0.3')
print()

# Тест линейного нейрона
print("Линейный нейрон:")
x = np.random.random((10, 2))
w = np.array([1, -1])
y_out1 = linear_neuron(x=x, w=w, bias=0.3)
print(f'Ответ нейрона: {y_out1}')
print('=> Нейрон вычисляет скалярное произведение входов на веса плюс смещение')
print()

# Тест сигмоидного нейрона
print("Сигмоидный нейрон:")
x = np.linspace(-10, 10.1, 20).reshape((20, 1))
w = np.array([0.9]).reshape((1, 1))
y_out2 = sigmoid_neuron(x=x, w=w, bias=0.3)
print(f'Ответ нейрона: {y_out2}')
print()

# Тест тангенциального нейрона
print("Тангенциальный нейрон:")
x = np.linspace(-10, 10.1, 20).reshape((20, 1))
w = np.array([0.9]).reshape((1, 1))
y_out3 = sigmoidt_neuron(x=x, w=w, bias=0.3)
print(f'Ответ нейрона: {y_out3}')
print()

# ============================================
# ВИЗУАЛИЗАЦИЯ ВСЕХ НЕЙРОНОВ (ЗАДАНИЕ 1)
# ============================================

print("Визуализация нейронов...")
x = np.linspace(-10, 10.1, 20).reshape((20, 1))
w = np.array([0.9]).reshape((1, 1))
bias = 0.3

y_out1 = McCuloh_Pitts_neuron(x=x, w=w, bias=0.3)
y_out2 = sigmoid_neuron(x=x, w=w, bias=0.3)
y_out3 = sigmoidt_neuron(x=x, w=w, bias=0.3)
y_out4 = linear_neuron(x=x, w=w, bias=0.3)

plt.plot(x[:, 0], y_out1[0, :], 'r', label='McCulloh-Pitts', linewidth=2)
plt.plot(x[:, 0], y_out4[0, :], 'k', label='Linear', linewidth=2)
plt.plot(x[:, 0], y_out2[0, :], 'b', label='Sigmoid', linewidth=2)
plt.plot(x[:, 0], y_out3[0, :], 'g', label='Tanh', linewidth=2)

plt.legend(fontsize=12)
plt.xlabel('x', fontsize=12)
plt.ylabel('y', fontsize=12)
plt.title('Сравнение функций активации', fontsize=14)
plt.grid(True, alpha=0.3)
plt.show() 
print()

# ============================================
# Сравнительный анализ
# ============================================

print("Графики производных активационных функций...")

def derivative_sigmoid(x):
    s = 1 / (1 + np.exp(-x))
    return s * (1 - s)

def derivative_tanh(x):
    return 1 - np.tanh(x)**2

x = np.linspace(-5, 5, 100)

plt.figure(figsize=(10, 6))
plt.plot(x, np.zeros_like(x), 'r--', label="Sign' = 0", linewidth=2)
plt.plot(x, np.ones_like(x), 'k--', label="Linear' = 1", linewidth=2)
plt.plot(x, derivative_sigmoid(x), 'b--', label="Sigmoid' = s(1-s)", linewidth=2)
plt.plot(x, derivative_tanh(x), 'g--', label="Tanh' = 1-tanh²(x)", linewidth=2)
plt.axhline(y=0, color='black', linestyle='-', alpha=0.2)
plt.axvline(x=0, color='black', linestyle='-', alpha=0.2)
plt.legend()
plt.title('Производные активационных функций')
plt.xlabel('x')
plt.ylabel("f'(x)")
plt.grid(True, alpha=0.3)
plt.show()
print()

print("Сравнительный анализ:")
print("- Sign: производная = 0 (кроме точки 0), недифференцируема")
print("- Linear: производная = 1 (константа), простая")
print("- Sigmoid: производная = s(1-s), максимум 0.25, гладкая")
print("- Tanh: производная = 1-tanh²(x), максимум 1, симметричная")
print()

# ============================================
# ЗАДАНИЕ 2: Двухмерный нейрон
# ============================================

print("="*60)
print("ЗАДАНИЕ 2: Двухмерный нейрон")
print("="*60)

# Строим поле координат 10x10
x1 = np.array([np.linspace(-10, 11, 10).reshape((10, 1))] * 10).reshape((10, 10))
x2 = x1.T
x2 = x2.reshape((100, 1))
x1 = x1.reshape((100, 1))
x = np.hstack([x1, x2])

print(f"Размерность данных: {x.shape}")
print()

# --- Визуализация нейрона МакКаллока-Питтса (веса [0.9, -0.9]) ---
w = np.array([0.9, -0.9]).reshape((1, 2))
y_out_21 = McCuloh_Pitts_neuron(x=x, w=w, bias=10)
y_out_21 = y_out_21.reshape((10, 10))

plt.imshow((y_out_21 ))
plt.title(f'Нейрон МаКаллока-Питтса 0.9, -0.9')
plt.colorbar()
plt.show()

w = np.array([-0.9, 0.9]).reshape((1, 2))
y_out_22 = McCuloh_Pitts_neuron(x=x, w=w, bias=10)
y_out_22 = y_out_22.reshape((10,10))

plt.imshow((y_out_22 ))
plt.title(f'Нейрон МаКаллока-Питтса -0.9, 0.9')
plt.colorbar()
plt.show()

# --- Сигмоидный нейрон с разными λ ---
print("Сигмоидный нейрон с разными λ:")
for i in range(5):
    lam = 0.1 + i * 0.5
    w = np.array([0.9, -0.1]).reshape((1, 2))
    y = sigmoidt_complex_neuron(x=x, w=w, bias=33.9, lymb=lam)
    y = y.reshape((10, 10))
    
    plt.figure(figsize=(6, 5))
    plt.imshow((y + 1) / 2, cmap='viridis', interpolation='bilinear', vmin=0, vmax=1)
    plt.colorbar(label='Выход [0,1]')
    plt.title(f'λ = {lam:.2f}')
    plt.xlabel('x1')
    plt.ylabel('x2')
    plt.show()
    print(f"График для λ={lam:.2f} отображен")
print()

# ============================================
# Два пространства из 10 объектов
# ============================================

print("Создание двух пространств из 10 объектов...")

np.random.seed(42)

# а) Линейно разделимые объекты
x1_lin = np.random.randn(5, 2) + [3, 3]
x2_lin = np.random.randn(5, 2) + [-3, -3]
X_lin = np.vstack([x1_lin, x2_lin])
D_lin = np.hstack([np.ones(5), -np.ones(5)])

# б) Линейно НЕ разделимые объекты
x1_nonlin = np.random.randn(5, 2) + [0, 2]
x2_nonlin = np.random.randn(5, 2) + [0, -2]
X_nonlin = np.vstack([x1_nonlin, x2_nonlin])
D_nonlin = np.hstack([np.ones(5), -np.ones(5)])

# Визуализация
plt.figure(figsize=(12, 5))
plt.subplot(1, 2, 1)
plt.plot(x1_lin[:,0], x1_lin[:,1], 'or', label='класс 1')
plt.plot(x2_lin[:,0], x2_lin[:,1], 'og', label='класс -1')
plt.legend()
plt.xlabel('x1')
plt.ylabel('x2')
plt.title('Линейно разделимые объекты')
plt.grid(True, alpha=0.3)

plt.subplot(1, 2, 2)
plt.plot(x1_nonlin[:,0], x1_nonlin[:,1], 'or', label='класс 1')
plt.plot(x2_nonlin[:,0], x2_nonlin[:,1], 'og', label='класс -1')
plt.legend()
plt.xlabel('x1')
plt.ylabel('x2')
plt.title('Линейно НЕ разделимые объекты')
plt.grid(True, alpha=0.3)
plt.show()
print()

# ============================================
# ЗАДАНИЕ 3: Обучение по Дельта-правилу
# ============================================

print("="*60)
print("ЗАДАНИЕ 3: Обучение по Дельта-правилу")
print("="*60)

# Входное пространство
x1 = np.array([1, 1, -1, -1])
x2 = np.array([1, -1, -1, 1])
x = np.vstack([x1, x2]).T
D = np.array([1, 1, -1, -1])

print(f"Входные данные X:\n{x}")
print(f"Целевые значения D: {D}")
print()

# Визуализация данных
plt.figure(figsize=(12, 4))

plt.subplot(1, 2, 1)
plt.plot(x1[:2], x2[:2], 'or', label='класс 1', markersize=10)
plt.plot(x1[2:4], x2[2:4], 'og', label='класс -1', markersize=10)
plt.legend(fontsize=12)
plt.xlabel('x1')
plt.ylabel('x2')
plt.title('Примеры объектов')
plt.grid(True, alpha=0.3)

plt.subplot(1, 2, 2)
plt.plot(D, 'bo-', linewidth=2, markersize=8)
plt.title('Целевой выход')
plt.xlabel('номер объекта')
plt.ylabel('D')
plt.grid(True, alpha=0.3)

plt.tight_layout()
plt.show()  # <--- ВАЖНО
print()

# Обучение нейрона
w = np.array([0.1, -0.1])
yt = linear_neuron(x=x, w=w, bias=0)

plt.figure(figsize=(10, 6))
plt.plot(yt, 'r', label='выходы до обучения', linewidth=2, marker='o')

# Обучение
for k in range(3):
    for i in range(4):
        e = yt[i] - D[i]
        dw = -0.1 * e * x[i, :]
        w = w + dw
        yt = linear_neuron(x=x, w=w, bias=0)
        plt.plot(yt, '--b', alpha=0.3)

plt.plot(yt, 'g', label='выходы после обучения', linewidth=3, marker='s')
plt.legend(fontsize=12)
plt.xlabel('Номер примера')
plt.ylabel('Выход нейрона')
plt.title('Процесс обучения по Дельта-правилу')
plt.grid(True)
plt.show() 
print()

# ============================================
# Обучение на двух пространствах
# ============================================

print("Обучение на двух пространствах...")

# Функция обучения
def train_neuron_simple(X, D, epochs=50, lr=0.1):
    w = np.array([0.1, -0.1])
    weights_history = [w.copy()]
    errors = []
    
    for epoch in range(epochs):
        yt = linear_neuron(x=X, w=w, bias=0)
        for i in range(len(D)):
            e = yt[i] - D[i]
            dw = -lr * e * X[i, :]
            w = w + dw
            yt = linear_neuron(x=X, w=w, bias=0)
            weights_history.append(w.copy())
        errors.append(np.mean((yt - D)**2))
    
    return w, np.array(weights_history), errors

# Обучение на линейно разделимых
print("Линейно разделимые:")
w_lin, w_hist_lin, err_lin = train_neuron_simple(X_lin, D_lin)
print(f"Конечные веса: {w_lin}")
print(f"Конечная ошибка: {err_lin[-1]:.6f}")

# Обучение на линейно НЕ разделимых
print("Линейно НЕ разделимые:")
w_nonlin, w_hist_nonlin, err_nonlin = train_neuron_simple(X_nonlin, D_nonlin)
print(f"Конечные веса: {w_nonlin}")
print(f"Конечная ошибка: {err_nonlin[-1]:.6f}")
print()

# ============================================
# Траектория изменения весов
# ============================================

print("Траектория изменения весов:")

plt.figure(figsize=(12, 5))
plt.subplot(1, 2, 1)
plt.plot(w_hist_lin[:, 0], 'r', label='w1')
plt.plot(w_hist_lin[:, 1], 'b', label='w2')
plt.title('Линейно разделимые: изменение весов')
plt.xlabel('Шаг обучения')
plt.ylabel('Значение веса')
plt.legend()
plt.grid(True, alpha=0.3)

plt.subplot(1, 2, 2)
plt.plot(w_hist_nonlin[:, 0], 'r', label='w1')
plt.plot(w_hist_nonlin[:, 1], 'b', label='w2')
plt.title('Линейно НЕ разделимые: изменение весов')
plt.xlabel('Шаг обучения')
plt.ylabel('Значение веса')
plt.legend()
plt.grid(True, alpha=0.3)
plt.show()
print()

# ============================================
# Оценка характера изменения весов
# ============================================

print("="*60)
print("ОЦЕНКА ХАРАКТЕРА ИЗМЕНЕНИЯ ВЕСОВ")
print("="*60)

print("\nЛинейно разделимые данные:")
print(f"  Начальные веса: w1 = {w_hist_lin[0,0]:.4f}, w2 = {w_hist_lin[0,1]:.4f}")
print(f"  Конечные веса: w1 = {w_hist_lin[-1,0]:.4f}, w2 = {w_hist_lin[-1,1]:.4f}")
print(f"  Изменение w1: {w_hist_lin[-1,0] - w_hist_lin[0,0]:+.4f}")
print(f"  Изменение w2: {w_hist_lin[-1,1] - w_hist_lin[0,1]:+.4f}")
print(f"  Конечная ошибка: {err_lin[-1]:.6f}")
print("  Характер: плавная сходимость, веса стабилизируются")

print("\nЛинейно НЕ разделимые данные:")
print(f"  Начальные веса: w1 = {w_hist_nonlin[0,0]:.4f}, w2 = {w_hist_nonlin[0,1]:.4f}")
print(f"  Конечные веса: w1 = {w_hist_nonlin[-1,0]:.4f}, w2 = {w_hist_nonlin[-1,1]:.4f}")
print(f"  Изменение w1: {w_hist_nonlin[-1,0] - w_hist_nonlin[0,0]:+.4f}")
print(f"  Изменение w2: {w_hist_nonlin[-1,1] - w_hist_nonlin[0,1]:+.4f}")
print(f"  Конечная ошибка: {err_nonlin[-1]:.6f}")
print("  Характер: колебания, веса не сходятся к стабильным значениям")
print()

# ============================================
# ЗАДАНИЕ 4: Линейная разделимость
# ============================================

print("="*60)
print("ЗАДАНИЕ 4: Линейная разделимость")
print("="*60)

# Задаем примеры на вход
x1 = np.array([1, 1, -1, -1])
x2 = np.array([1, -1, -1, 1])
x = np.vstack([x1, x2]).T
D = np.array([1, -1, 1, -1])

print(f"Входные данные X:\n{x}")
print(f"Целевые значения D: {D}")
print()

# Визуализация
plt.figure(figsize=(12, 4))

plt.subplot(1, 2, 1)
plt.plot(x1[[1, 3]], x2[[1, 3]], 'or', label='класс 1', markersize=10)
plt.plot(x1[[0, 2]], x2[[0, 2]], 'og', label='класс -1', markersize=10)
plt.legend(fontsize=12)
plt.xlabel('x1')
plt.ylabel('x2')
plt.title('Примеры объектов')
plt.grid(True, alpha=0.3)

plt.subplot(1, 2, 2)
plt.plot(D, 'bo-', linewidth=2, markersize=8)
plt.title('Целевой выход')
plt.xlabel('номер объекта')
plt.ylabel('D')
plt.grid(True, alpha=0.3)

plt.tight_layout()
plt.show()  # <--- ВАЖНО
print()

# Обучение
w = np.array([0.1, -0.1])
yt = linear_neuron(x=x, w=w, bias=0)

plt.figure(figsize=(10, 6))
plt.plot(yt, 'r', label='выходы до обучения', linewidth=2, marker='o')

for k in range(3):
    for i in range(4):
        e = yt[i] - D[i]
        dw = -0.1 * e * x[i, :]
        w = w + dw
        yt = linear_neuron(x=x, w=w, bias=0)
        plt.plot(yt, '--b', alpha=0.3)

plt.plot(yt, 'g', label='выходы после обучения', linewidth=3, marker='s')
plt.legend(fontsize=12)
plt.xlabel('Номер примера')
plt.ylabel('Выход нейрона')
plt.title('Обучение по Дельта-правилу (Задание 4)')
plt.grid(True)
plt.show()  # <--- ВАЖНО
print()

print(f"Финальные веса: {w}")
print(f"Финальные выходы: {yt}")
print()

# ============================================
# ОБЪЕДИНЕНИЕ НЕЙРОНОВ
# ============================================

print("="*60)
print("Объединение нейронов")
print("="*60)

try:
    x_y2 = np.hstack([y_out_21.reshape((100, 1)), y_out_22.reshape((100, 1))])
    print(f"Размерность объединенных данных: {x_y2.shape}")
    print("Данные для третьего нейрона готовы")
    
    # Визуализация объединенных данных
    plt.figure(figsize=(10, 4))
    plt.subplot(1, 2, 1)
    plt.imshow(y_out_21, cmap='coolwarm', interpolation='nearest')
    plt.title('Нейрон 1 [0.9, -0.9]')
    plt.colorbar()
    
    plt.subplot(1, 2, 2)
    plt.imshow(y_out_22, cmap='coolwarm', interpolation='nearest')
    plt.title('Нейрон 2 [-0.9, 0.9]')
    plt.colorbar()
    plt.tight_layout()
    plt.show()  # <--- ВАЖНО
    
except NameError as e:
    print(f"Ошибка: {e}")
    print("Переменные y_out_21 и y_out_22 не найдены")
    print("Убедитесь, что Задание 2 было выполнено")

print()
print("="*60)
print("ВСЕ ЗАДАНИЯ ВЫПОЛНЕНЫ!")
print("="*60)