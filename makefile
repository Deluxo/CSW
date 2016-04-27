target: csw.cs
	mcs csw.cs -out:bin/csw-linux
clean:
	rm bin/csw-linux
install:
	mcs csw.cs -out:bin/csw-linux
	sudo cp bin/csw-linux /bin/csw
remove:
	sudo rm /bin/csw
