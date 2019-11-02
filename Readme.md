# Dotnet Konf'19 Lab

[Presentation](https://speakerdeck.com/selcukusta/effecient-ways-of-implementation-secure-http-in-microservices?slide=35)

## Create Sample Self-Signed Certificates

`openssl req -newkey rsa:2048 -nodes -x509 -out certs/dotnet-konf_sample_com.pem -keyout certs/dotnet-konf_sample_com.key -subj "/CN=dotnet-konf-sample.com/O=dotnet-konf-sample.com" -days 365`

`openssl pkcs12 -inkey certs/dotnet-konf_sample_com.key -in certs/dotnet-konf_sample_com.pem -export -out certs/dotnet-konf_sample_com.pfx`

## Mandatories (_Need to use Nginx Ingress Controller on Minikube_)

`kubectl apply -f .mandatory/1-nginx-ingress.yaml`

`kubectl apply -f .mandatory/2-nginx-ingress-nodeport.yaml`

## Scenario #1 (SSL Termination)

- Create certificate

`kubectl create secret tls ingress-tls --key certs/dotnet-konf_sample_com.key --cert certs/dotnet-konf_sample_com.pem`

- Create deployment and service

`kubectl apply -f No-Cert.yaml`

- Create ingress object

`kubectl apply -f .ingress-manifests/Valid-TLS-Secret-Termination-Ingress.yaml`

- Get `ingress-nginx` pod name to port-forward

`kubectl get pods -n ingress-nginx` _(Get pod name)_

- Use port forwarding for inspecting request/response

`kubectl port-forward [POD_NAME] 4443:443 -n ingress-nginx`

- Goto https://dotnet-konf-sample.com:4443/api/values and inspect the response body and headers!

- Delete all sample objects

```
kubectl delete -f .ingress-manifests/Valid-TLS-Secret-Termination-Ingress.yaml

kubectl delete -f No-Cert.yaml

kubectl delete secret ingress-tls
```

## Scenario #2 (SSL Passthrough)

- Create generic secret to store certificate

`kubectl create secret generic dotnet-konf-sample-com-file --from-file certs/dotnet-konf_sample_com.pfx`

- Create generic secre to store certificate password and path

`kubectl create secret generic dotnet-konf-sample-com-literal --from-literal=PWD=1 --from-literal=CERTIFICATE_PATH=certificates/dotnet-konf_sample_com.pfx`

- Create deployment and service

`kubectl apply -f Cert-Excluded.yaml`

- Create ingress object

`kubectl apply -f .ingress-manifests/No-TLS-Secret-Passthrough-Ingress.yaml`

- Get `ingress-nginx` pod name to port-forward

`kubectl get pods -n ingress-nginx` _(Get pod name)_

- Use port forwarding for inspecting request/response

`kubectl port-forward [POD_NAME] 4443:443 -n ingress-nginx`

- Goto https://dotnet-konf-sample.com:4443/api/values and inspect the response body and headers!

- Delete all sample objects

```
kubectl delete -f .ingress-manifests/No-TLS-Secret-Passthrough-Ingress.yaml

kubectl delete -f Cert-Excluded.yaml

kubectl delete secret dotnet-konf-sample-com-literal

kubectl delete secret dotnet-konf-sample-com-file
```
