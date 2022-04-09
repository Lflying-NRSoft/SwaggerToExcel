# docker run -d --rm -p 9007:80 --name dtmdemostock registry.cn-shanghai.aliyuncs.com/nrsoft/dtmdemostock:develop

FROM ccr.ccs.tencentyun.com/magicodes/aspnetcore-runtime:6.0withfonts AS base
WORKDIR /app
copy . ./ 
EXPOSE 80 443

#RUN sed -i 's/TLSv1.2/TLSv1.0/g' /etc/ssl/openssl.cnf
#RUN sed -i 's/TLSv1.2/TLSv1/g' /etc/ssl/openssl.cnf

ENTRYPOINT ["dotnet", "NRSoft.SwaggerToExcel.HttpApi.Host.dll"]
