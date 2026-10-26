# IF133 - Tugas 1: Backend Skeleton & Data Persistence

## Identitas
- **Nama**: Stefanus Dimas Freynando Gabriel
- **NIM**: 1003240022
- **Kelas / Prodi**: S1 Informatika (IF)
- **Mata Kuliah**: IF133 - Pemrograman .NET (Pertemuan 4)

## Studi Kasus
Studi kasus yang dipilih adalah **Katalog Produk & Inventaris Toko (Product / Inventory)**.  
Entity utama yang dimodelkan adalah `Product` untuk mencatat dan mengelola data barang secara persisten ke dalam basis data relasional SQLite.

### Rincian Field Entity `Product`:
| Field | Tipe Data | Wajib | Keterangan & Validasi |
|---|---|---|---|
| `Id` | `int` | Ya | Primary Key, Auto-increment |
| `Name` | `string` | Ya | Nama produk barang, tidak boleh kosong, maksimal 100 karakter (`[Required]`, `[StringLength(100)]`) |
| `Description` | `string?` | Tidak | Deskripsi singkat atau spesifikasi produk (opsional / *nullable*) |
| `Price` | `decimal` | Ya | Harga produk dalam Rupiah, nilai harus $\ge 0$ (`[Required]`, `[Range(0, double.MaxValue)]`) |
| `Stock` | `int` | Ya | Jumlah stok fisik barang, nilai harus $\ge 0$ (`[Required]`, `[Range(0, int.MaxValue)]`) |

## Teknologi
- **ASP.NET Core Web API** (.NET 8.0 SDK berbasis Controller)
- **Entity Framework Core 8.0** (`Microsoft.EntityFrameworkCore.Sqlite`, `Microsoft.EntityFrameworkCore.Design`)
- **Database Provider**: SQLite (`products.db`)
- **Swagger / OpenAPI**: Dokumentasi dan antarmuka pengujian API (`Swashbuckle.AspNetCore`)

## Cara Menjalankan
1. **Restore dependency:**
   ```powershell
   dotnet restore
   ```
2. **Terapkan migrasi ke database (opsional jika products.db sudah ada):**
   ```powershell
   dotnet ef database update --project IF133.Tugas1.Api
   ```
3. **Jalankan project:**
   ```powershell
   dotnet run --project IF133.Tugas1.Api --launch-profile http
   ```
4. **Buka Swagger UI di browser:**
   ```text
   http://localhost:5176/swagger
   ```

## Endpoint
Daftar endpoint RESTful API berbasis resource `/api/products`:

| Operasi | HTTP Method | Endpoint URI | Body Request | Status Sukses | Status Error |
|---|:---:|---|---|:---:|:---:|
| **Read All** | `GET` | `/api/products` | *(None)* | `200 OK` | `500` |
| **Read One** | `GET` | `/api/products/{id}` | *(None)* | `200 OK` | `404 Not Found` |
| **Create** | `POST` | `/api/products` | JSON (`Product`) | `201 Created` | `400 Bad Request` |
| **Update** | `PUT` | `/api/products/{id}` | JSON (`Product`) | `200 OK` | `400 Bad Request`, `404 Not Found` |
| **Delete** | `DELETE` | `/api/products/{id}` | *(None)* | `204 No Content` | `404 Not Found` |

## Bukti Pengujian

Pengujian dilakukan menggunakan Swagger UI dan HTTP Client terhadap skenario wajib:

| No | Skenario Pengujian | Method | Endpoint | Status Code | Keterangan / Hasil |
|:--:|---|:---:|---|:---:|---|
| 1 | GET seluruh data (kondisi awal) | `GET` | `/api/products` | `200 OK` | Mengembalikan list data produk |
| 2 | POST data valid | `POST` | `/api/products` | `201 Created` | Produk baru berhasil disimpan dengan ID auto-increment |
| 3 | GET ID valid | `GET` | `/api/products/1` | `200 OK` | Mengembalikan data detail produk ID 1 |
| 4 | PUT data valid | `PUT` | `/api/products/1` | `200 OK` | Data produk ID 1 berhasil diperbarui |
| 5 | DELETE data valid | `DELETE` | `/api/products/1` | `204 No Content` | Data produk ID 1 berhasil dihapus permanen |
| 6 | GET ID tidak ada (ID 999) | `GET` | `/api/products/999` | `404 Not Found` | Skenario Gagal: ID tidak ditemukan di database |
| 7 | POST data tidak valid | `POST` | `/api/products` | `400 Bad Request` | Skenario Gagal: Ditolak karena Name kosong / Price < 0 |

### Contoh Request dan Response:

#### 1. POST /api/products (Data Valid - 201 Created)
- **Request Body:**
  ```json
  {
    "name": "Keyboard Mechanical",
    "description": "Keyboard 75% Layout Wireless",
    "price": 350000,
    "stock": 15
  }
  ```
- **Response:** `201 Created`
  ```json
  {
    "id": 1,
    "name": "Keyboard Mechanical",
    "description": "Keyboard 75% Layout Wireless",
    "price": 350000,
    "stock": 15
  }
  ```

#### 2. GET /api/products/999 (Resource Not Found - 404 Not Found)
- **Response:** `404 Not Found`
  ```json
  {
    "message": "Produk dengan ID 999 tidak ditemukan."
  }
  ```

#### 3. POST /api/products (Data Tidak Valid - 400 Bad Request)
- **Request Body:**
  ```json
  {
    "name": "",
    "description": "Produk Error",
    "price": -50000,
    "stock": -5
  }
  ```
- **Response:** `400 Bad Request`
  ```json
  {
    "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
    "title": "One or more validation errors occurred.",
    "status": 400,
    "errors": {
      "Name": ["Name wajib diisi."],
      "Price": ["Price harus bernilai >= 0."],
      "Stock": ["Stock harus bernilai >= 0."]
    }
  }
  ```

## Kendala dan Solusi
1. **Kendala 1: Kebutuhan Global Tools dan SDK:**
   - *Masalah:* Perintah migrasi `dotnet ef` membutuhkan tool Entity Framework Core yang terinstal pada sistem.
   - *Solusi:* Memasang paket `Microsoft.EntityFrameworkCore.Design` langsung pada file `.csproj` sehingga perintah migrasi dapat dieksekusi secara lokal dan terpercaya.
2. **Kendala 2: Validasi Integritas Data Masukan:**
   - *Masalah:* Risiko input data produk tanpa nama atau memiliki nilai harga/stok negatif.
   - *Solusi:* Menerapkan validasi deklaratif *Data Annotations* (`[Required]`, `[StringLength]`, `[Range]`) pada model Entity `Product`, sehingga ASP.NET Core otomatis memvalidasi payload sebelum diproses ke database dan mengembalikan respon `400 Bad Request` jika tidak valid.
