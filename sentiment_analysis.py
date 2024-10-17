# Import các thư viện cần thiết
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import seaborn as sns
import warnings
from sklearn.model_selection import train_test_split
from sklearn.feature_extraction.text import CountVectorizer
from sklearn.naive_bayes import MultinomialNB
from sklearn.metrics import accuracy_score, classification_report, confusion_matrix

# Tắt cảnh báo để tránh bị làm phiền
warnings.filterwarnings("ignore")

# Đọc dữ liệu từ tệp CSV
data = pd.read_csv(r"C:\dataset\synthetic_social_media_data.csv")

# Kiểm tra và hiển thị danh sách các cột
print("Danh sách các cột trong dataset:")
print(data.columns)

# Sử dụng đúng tên cột nếu khác biệt
data = data.dropna(subset=['Post Content', 'Sentiment Label'])  # Bỏ các hàng thiếu

# Tách đầu vào và đầu ra
X = data['Post Content']  # Đầu vào
y = data['Sentiment Label']  # Đầu ra

# Chia dữ liệu
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

# Chuyển văn bản thành vectơ đặc trưng bằng CountVectorizer
vectorizer = CountVectorizer()
X_train_vec = vectorizer.fit_transform(X_train)
X_test_vec = vectorizer.transform(X_test)

# Khởi tạo và huấn luyện mô hình Naive Bayes
model = MultinomialNB()
model.fit(X_train_vec, y_train)

# Dự đoán trên tập kiểm tra
y_pred = model.predict(X_test_vec)

# Đánh giá mô hình bằng độ chính xác và báo cáo phân loại
print(f'\nĐộ chính xác của mô hình: {accuracy_score(y_test, y_pred):.2f}')
print('\nBáo cáo phân loại:')
print(classification_report(y_test, y_pred))

# Hiển thị ma trận nhầm lẫn dưới dạng biểu đồ
plt.figure(figsize=(8, 6))
conf_matrix = confusion_matrix(y_test, y_pred)
sns.heatmap(conf_matrix, annot=True, cmap='coolwarm', fmt='d', cbar=False)
plt.title('Ma trận nhầm lẫn')
plt.xlabel('Dự đoán')
plt.ylabel('Thực tế')
plt.show()

# Hiển thị 5 dòng đầu tiên với nhãn dự đoán và nhãn thực tế
predicted_data = pd.DataFrame({'Văn bản': X_test.values, 
                               'Cảm xúc thực tế': y_test.values, 
                               'Cảm xúc dự đoán': y_pred})
print("\nHiển thị 5 dòng đầu tiên của dữ liệu đã được dự đoán:")
print(predicted_data.head())

# biểu đồ tròn tỷ lệ các nhãn cảm xúc
sentiment_counts = data['Sentiment Label'].value_counts()
plt.figure(figsize=(8, 6))
plt.pie(sentiment_counts, labels=sentiment_counts.index, autopct='%1.1f%%', startangle=140, colors=['#FF9999', '#66B3FF', '#99FF99'])
plt.title('Tỷ lệ các nhãn cảm xúc')
plt.axis('equal')  # Để đảm bảo hình tròn
plt.show()


#biểu đồ phân phối bài đăng theo cảm xúc
plt.figure(figsize=(10, 6))
sns.countplot(x='Sentiment Label', hue='Post Type', data=data, palette='coolwarm')
plt.title('Phân phối loại bài đăng theo nhãn cảm xúc')
plt.xlabel('Nhãn cảm xúc')
plt.ylabel('Số lượng bài đăng')
plt.show()
