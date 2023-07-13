import os
import openai
import re  # 导入正则表达式模块
import sys
import time
import json
import docx2txt

start_time = time.time()  # 添加这行代码
print(start_time)




def docx_to_txt(input_docx_path, output_txt_path):
    # 使用docx2txt库读取docx文件内容
    text = docx2txt.process(input_docx_path)

    # 删除重复行
    lines = text.split('\n')
    unique_lines = []
    for line in lines:
        if line not in unique_lines:
            unique_lines.append(line)
    text = '\n'.join(unique_lines)

    # 将读取到的问题内容写入txt文件
    with open(output_txt_path, 'w', encoding='utf-8') as file:
        file.write(text)




# 导入所需的库
from pdfminer.pdfparser import PDFParser, PDFDocument
from pdfminer.pdfdevice import PDFDevice
from pdfminer.pdfinterp import PDFResourceManager, PDFPageInterpreter
from pdfminer.converter import PDFPageAggregator
from pdfminer.layout import LTTextBoxHorizontal, LAParams
from pdfminer.pdfinterp import PDFTextExtractionNotAllowed



# 定义函数，接受输入的PDF文件路径和输出的txt文件路径
def pdf_to_txt(input_pdf_path, output_txt_path):

    open(output_txt_path, 'w').close()  # 清空txt文件

    # 定义内部函数parse，处理PDF文件
    def parse(input_pdf_file, output_txt_file):
        
        # 用文件对象创建一个PDF文档分析器
        parser = PDFParser(input_pdf_file)
        # 创建一个PDF文档
        doc = PDFDocument()
        # 分析器和文档相互连接
        parser.set_document(doc)
        doc.set_parser(parser)
        # 提供初始化密码，没有默认为空
        doc.initialize()

        # 检查文档是否可以转成TXT，如果不可以就忽略
        if not doc.is_extractable:
            raise PDFTextExtractionNotAllowed
        else:
            # 创建PDF资源管理器，来管理共享资源
            rsrcmgr = PDFResourceManager()
            # 创建一个PDF设备对象
            laparams = LAParams()
            # 将资源管理器和设备对象聚合
            device = PDFPageAggregator(rsrcmgr, laparams=laparams)
            # 创建一个PDF解释器对象
            interpreter = PDFPageInterpreter(rsrcmgr, device)

            # 循环遍历列表，每次处理一个page内容
            for page in doc.get_pages():
                interpreter.process_page(page)
                # 接收该页面的LTPage对象
                layout = device.get_result()
                
                for x in layout:
                    try:
                        if isinstance(x, LTTextBoxHorizontal):
                            with open(output_txt_file, 'a', encoding='utf-8-sig') as f:
                                result = x.get_text()
                                # 删除任何前导/尾随的空格
                                result = result.strip()
                                # 如果行不为空，则写入文件
                                if result != '':
                                    f.write(result + "\n")
                    except:
                        print("Failed")

    # 打开并处理PDF文件
    with open(input_pdf_path, 'rb') as pdf_file:
        parse(pdf_file, output_txt_path)







from ecloud import CMSSEcloudOcrClient
import json

accesskey = '4863f884aef84ea4a4af9895285b75ec' 
secretkey = '249b66cddeaa453f8c3689761476b08a'
url = 'https://api-wuxi-1.cmecloud.cn:8443'


def img_to_txt(input_img_path, output_txt_path):
    print("正在从图片转化为txt")
    print(input_img_path)
    print(output_txt_path)
    requesturl = '/api/ocr/v1/webimage'
    try:
        ocr_client = CMSSEcloudOcrClient(accesskey, secretkey, url)
        response = ocr_client.request_ocr_service_file(requestpath=requesturl, imagepath=input_img_path)

        response_json = json.loads(response.text)  # 解析JSON
        words_info = response_json['body']['content']['prism_wordsInfo']  # 取出所有识别出的文字的信息

        with open(output_txt_path, 'w', encoding='utf-8') as file:
            for word_info in words_info:
                file.write(word_info['word'] + '\n')  # 将识别出的文字写入到文件中
    except ValueError as e:
        print(e)
    return output_txt_path




os.environ["OPENAI_API_BASE"]="https://api.openai.com/v1"
openai.api_key = "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"


# 读取命令行参数

filepath = sys.argv[1].strip()
print(filepath)
r_id = sys.argv[2].strip()
print(r_id)
extention = sys.argv[3].replace(' ', '')

print(extention)
n = sys.argv[4].strip()
print(n)

# # # print(filepath)
# 使用 os.path.dirname 和 os.path.join 构建新的路径
base_dir = os.path.dirname(filepath)
prompt_txt = os.path.join(base_dir, f'{n}.txt')



print(os.path.join(filepath, f'{r_id}{extention}'))
print(os.path.join(base_dir, "txt", f'{r_id}.txt'))


# 根据文件扩展名将文件转换为txt
# 根据文件扩展名将文件转换为txt
if extention == ".docx":
    docx_to_txt(os.path.join(filepath, f'{r_id}{extention}'), os.path.join(base_dir, "txt", f'{r_id}.txt'))
elif extention == ".pdf":
    pdf_to_txt(os.path.join(filepath, f'{r_id}{extention}'), os.path.join(base_dir, "txt", f'{r_id}.txt'))
elif extention in [".img", ".jpg", ".png"]:
    print("正在从图片转化为txt")    
    print(os.path.join(filepath, f'{r_id}{extention}'))
    print(os.path.join(base_dir, "txt", f'{r_id}.txt'))
    img_to_txt(os.path.join(filepath, f'{r_id}{extention}'), os.path.join(base_dir, "txt", f'{r_id}.txt'))
else:
    print("文件格式不支持")


print(base_dir)

# 现在，resume_txt将指向新生成的txt文件
resume_txt_path = os.path.join(base_dir, f'txt\\{r_id}.txt')
print(resume_txt_path)


# 打开文件并读取内容
with open(resume_txt_path, 'r', encoding='utf-8') as file: 
    resume_text = file.read()


def openai_request(prompt):
    message = [
        {"role": "system", "content": prompt},
        {"role": "user", "content": f"我这里有一份简历，我需要获取其中的一些信息。简历如下：{resume_text}"},
    ]

    response = openai.ChatCompletion.create(
        model="gpt-3.5-turbo-16k",
        messages=message,
        temperature=0.01,
        max_tokens=3000,
        top_p=1,
        frequency_penalty=0,
        presence_penalty=0,
    )


    return response['choices'][0]['message']['content']


# 读取txt的prompt文件
with open(prompt_txt, 'r', encoding='utf-8') as file:
    prompt = file.read().replace('\n', '')
answer = openai_request(prompt)
print(answer)




end_time = time.time()  # 添加这行代码
print(end_time)

print('Total execution time: ' + str(end_time - start_time) + ' seconds')  # 添加这行代码