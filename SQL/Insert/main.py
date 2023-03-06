import aiohttp
import asyncio
import os
import aiofiles

async def download_image(url, headers):
    try:
        async with aiohttp.ClientSession(headers=headers) as session:
            async with session.get(url) as response:
                if response.status == 200:
                    filename = os.path.join('C:/base/stud/actual/Supermarket CRM/SQL/Insert/images', url.split('/')[-1])
                    async with aiofiles.open(filename, mode='wb') as f:
                        await f.write(await response.read())
                    print(f'Successfully downloaded {filename}')
                else:
                    filename = os.path.join('C:/base/stud/actual/Supermarket CRM/SQL/Insert/images', url.split('/')[-1])
                    async with aiofiles.open(filename, mode='wb') as f:
                        await f.write(await response.read())
                    print(f'Failed to download {filename}: Status code {response.status}')
    except Exception as e:
        # #print(f'Failed to download {filename}: {e}')
        # filename = os.path.join('C:/base/stud/actual/Supermarket CRM/SQL/Insert/images', url.split('/')[-1])
        # async with aiofiles.open(filename, mode='wb') as f:
        #     await f.write(image_not_found)
        print(e)

async def main():
    headers = {
        'Host': 'www.bigbasket.com',
        'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/111.0',
        'Accept': 'text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8',
        'Accept-Language': 'ru,en-US;q=0.7,en;q=0.3',
        'Accept-Encoding': 'gzip, deflate, br',
        'DNT': '1',
        'Upgrade-Insecure-Requests': '1',
        'Connection': 'keep-alive',
        'Sec-Fetch-Dest': 'document',
        'Sec-Fetch-Mode': 'navigate',
        'Sec-Fetch-Site': 'none',
        'Sec-Fetch-User': '?1',
        'Sec-GPC': '1'
    }
    tasks = []
    async with aiofiles.open('C:/base/stud/actual/Supermarket CRM/SQL/Insert/imageUrls.txt') as f:
        async for url in f:
            url = url.strip()
            tasks.append(asyncio.create_task(download_image(url, headers)))
    await asyncio.gather(*tasks)

if __name__ == '__main__':
    asyncio.run(main())


