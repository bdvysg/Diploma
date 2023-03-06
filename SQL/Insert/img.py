import pyodbc 
import io
from PIL import Image
conn = pyodbc.connect('Driver={SQL Server};'
                      'Server=BDVYSG\MSSQLSERVER01;'
                      'Database=Market;'
                      'Trusted_Connection=yes;')
 
cursor = conn.cursor()

cursor.execute("SELECT Pr_Image FROM Product WHERE Pr_Id = 634")
row = cursor.fetchone()

# convert the image data to a PIL Image object
image_bytes = io.BytesIO(row[0])
img = Image.open(image_bytes)

# save the image to a file and open it in an image viewer
img.save('myimage.jpg')
img.show()


cursor.close()
conn.close()