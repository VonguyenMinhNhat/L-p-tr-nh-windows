# Import các thư viện cần thiết
import tensorflow as tf
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import seaborn as sns
import warnings
from sklearn.model_selection import train_test_split
from sklearn.feature_extraction.text import CountVectorizer
from sklearn.naive_bayes import MultinomialNB
from sklearn.metrics import accuracy_score, classification_report, confusion_matrix
from sklearn.preprocessing import LabelEncoder

# Tắt cảnh báo để tránh bị làm phiền
warnings.filterwarnings("ignore")

# Đọc dữ liệu từ tệp CSV
data = pd.read_csv(r"C:\dataset\synthetic_social_media_data.csv")

# Kiểm tra và hiển thị danh sách các cột
print("Danh sách các cột trong dataset:")
for col in data.columns:
    print(col)

# Bỏ các hàng thiếu
data = data.dropna(subset=['Post Content', 'Sentiment Label'])

# Tách đầu vào và đầu ra
X = data['Post Content']  # Đầu vào
y = data['Sentiment Label']  # Đầu ra

# Mã hóa nhãn đầu ra
encoder = LabelEncoder()
y = encoder.fit_transform(y)
y = tf.keras.utils.to_categorical(y)

# Chia dữ liệu
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

# Chuyển đổi văn bản thành vectơ đặc trưng bằng CountVectorizer cho Naive Bayes
vectorizer = CountVectorizer()
X_train_vec = vectorizer.fit_transform(X_train)
X_test_vec = vectorizer.transform(X_test)

# Khởi tạo và huấn luyện mô hình Naive Bayes
nb_model = MultinomialNB()
nb_model.fit(X_train_vec, np.argmax(y_train, axis=1))

# Dự đoán trên tập kiểm tra bằng Naive Bayes
y_pred_nb = nb_model.predict(X_test_vec)

# Đánh giá mô hình bằng độ chính xác và báo cáo phân loại của Naive Bayes
print(f'\nĐộ chính xác của mô hình Naive Bayes: {accuracy_score(np.argmax(y_test, axis=1), y_pred_nb):.2f}')
print('\nBáo cáo phân loại của Naive Bayes:')
print(classification_report(np.argmax(y_test, axis=1), y_pred_nb))

# Hiển thị ma trận nhầm lẫn dưới dạng biểu đồ
plt.figure(figsize=(8, 6))
conf_matrix_nb = confusion_matrix(np.argmax(y_test, axis=1), y_pred_nb)
sns.heatmap(conf_matrix_nb, annot=True, cmap='coolwarm', fmt='d', cbar=False)
plt.title('Ma trận nhầm lẫn - Naive Bayes')
plt.xlabel('Dự đoán')
plt.ylabel('Thực tế')
plt.show()

# Chuyển đổi văn bản thành vectơ đặc trưng bằng Tokenizer của TensorFlow cho LSTM
tokenizer = tf.keras.preprocessing.text.Tokenizer(num_words=5000)
tokenizer.fit_on_texts(X_train)
X_train_seq = tokenizer.texts_to_sequences(X_train)
X_test_seq = tokenizer.texts_to_sequences(X_test)

# Đệm các vectơ đặc trưng
maxlen = 100
X_train_pad = tf.keras.preprocessing.sequence.pad_sequences(X_train_seq, maxlen=maxlen)
X_test_pad = tf.keras.preprocessing.sequence.pad_sequences(X_test_seq, maxlen=maxlen)

# Khởi tạo và huấn luyện mô hình học sâu với TensorFlow
model = tf.keras.Sequential([
    tf.keras.layers.Embedding(input_dim=5000, output_dim=128, input_length=maxlen),
    tf.keras.layers.LSTM(units=128, dropout=0.2, recurrent_dropout=0.2),
    tf.keras.layers.Dense(units=y.shape[1], activation='softmax')
])

model.compile(loss='categorical_crossentropy', optimizer='adam', metrics=['accuracy'])

# Hiển thị cấu trúc của mô hình
print("\nCấu trúc của mô hình học sâu:")
model.summary()

# Huấn luyện mô hình
model.fit(X_train_pad, y_train, epochs=5, batch_size=32, validation_data=(X_test_pad, y_test))

# Đánh giá mô hình
loss, accuracy = model.evaluate(X_test_pad, y_test)
print(f'\nĐộ chính xác của mô hình LSTM: {accuracy:.2f}')

# Dự đoán và hiển thị nhãn cảm xúc dự đoán
y_pred_lstm = model.predict(X_test_pad)
y_pred_classes = np.argmax(y_pred_lstm, axis=1)
y_test_classes = np.argmax(y_test, axis=1)

print('\nBáo cáo phân loại của LSTM:')
print(classification_report(y_test_classes, y_pred_classes, target_names=encoder.classes_))

# Hiển thị ma trận nhầm lẫn dưới dạng biểu đồ
plt.figure(figsize=(8, 6))
conf_matrix_lstm = confusion_matrix(y_test_classes, y_pred_classes)
sns.heatmap(conf_matrix_lstm, annot=True, cmap='coolwarm', fmt='d', cbar=False)
plt.title('Ma trận nhầm lẫn - LSTM')
plt.xlabel('Dự đoán')
plt.ylabel('Thực tế')
plt.show()

# Tạo hàm để chuyển đổi nhãn thành văn bản
def label_to_text(label):
    if label == 0:
        return 'Tiêu cực'
    elif label == 1:
        return 'Trung tính'
    elif label == 2:
        return 'Tích cực'
    return label

# Dự đoán kết quả và tạo DataFrame cho Naive Bayes
predicted_data_nb = pd.DataFrame({'Văn bản': X_test.values,
                                  'Cảm xúc thực tế': np.argmax(y_test, axis=1),
                                  'Cảm xúc dự đoán': y_pred_nb})
predicted_data_nb['Cảm xúc thực tế'] = predicted_data_nb['Cảm xúc thực tế'].apply(label_to_text)
predicted_data_nb['Cảm xúc dự đoán'] = predicted_data_nb['Cảm xúc dự đoán'].apply(label_to_text)

# Dự đoán kết quả và tạo DataFrame cho LSTM
predicted_data_lstm = pd.DataFrame({'Văn bản': X_test.values,
                                    'Cảm xúc thực tế': np.argmax(y_test, axis=1),
                                    'Cảm xúc dự đoán': y_pred_classes})
predicted_data_lstm['Cảm xúc thực tế'] = predicted_data_lstm['Cảm xúc thực tế'].apply(label_to_text)
predicted_data_lstm['Cảm xúc dự đoán'] = predicted_data_lstm['Cảm xúc dự đoán'].apply(label_to_text)

# Đảm bảo các cột được hiển thị căn lề trái
pd.set_option('display.colheader_justify', 'left')  # Đặt tùy chọn căn trái cho tiêu đề cột

# Hiển thị 12 dòng đầu tiên với nhãn dự đoán và nhãn thực tế của Naive Bayes
print("\nHiển thị 12 dòng đầu tiên của dữ liệu đã được dự đoán - Naive Bayes:")
print(predicted_data_nb.head(12))

# Hiển thị 12 dòng đầu tiên với nhãn dự đoán và nhãn thực tế của LSTM
print("\nHiển thị 12 dòng đầu tiên của dữ liệu đã được dự đoán - LSTM:")
print(predicted_data_lstm.head(12))

# Biểu đồ tròn tỷ lệ các nhãn cảm xúc cho Naive Bayes
sentiment_counts_nb = pd.Series(predicted_data_nb['Cảm xúc dự đoán']).value_counts()
plt.figure(figsize=(8, 6))
plt.pie(sentiment_counts_nb, labels=sentiment_counts_nb.index, autopct='%1.1f%%', startangle=140, colors=['#FF9999', '#66B3FF', '#99FF99'])
plt.title('Tỷ lệ các nhãn cảm xúc - Naive Bayes')
plt.axis('equal')  
plt.show()

# Biểu đồ tròn tỷ lệ các nhãn cảm xúc cho LSTM
sentiment_counts_lstm = pd.Series(predicted_data_lstm['Cảm xúc dự đoán']).value_counts()
plt.figure(figsize=(8, 6))
plt.pie(sentiment_counts_lstm, labels=sentiment_counts_lstm.index, autopct='%1.1f%%', startangle=140, colors=['#FF9999', '#66B3FF', '#99FF99'])
plt.title('Tỷ lệ các nhãn cảm xúc - LSTM')
plt.axis('equal')  
plt.show()

# Biểu đồ phân phối bài đăng theo cảm xúc cho Naive Bayes
plt.figure(figsize=(10, 6))
sns.countplot(x='Cảm xúc dự đoán', data=predicted_data_nb, palette='coolwarm')
plt.title('Phân phối loại bài đăng theo nhãn cảm xúc - Naive Bayes')
plt.xlabel('Nhãn cảm xúc')
plt.ylabel('Số lượng')
plt.show()

# Biểu đồ phân phối bài đăng theo cảm xúc cho LSTM
plt.figure(figsize=(10, 6))
sns.countplot(x='Cảm xúc dự đoán', data=predicted_data_lstm, palette='coolwarm')
plt.title('Phân phối loại bài đăng theo nhãn cảm xúc - LSTM')
plt.xlabel('Nhãn cảm xúc')
plt.ylabel('Số lượng')
plt.show()

