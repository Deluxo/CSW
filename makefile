target: csw.cs
	mcs csw.cs -out:bin/csw-linux
clean:
	rm bin/csw-linux
install:
	mcs csw.cs -out:bin/csw-linux
	sudo cp bin/csw-linux /bin/csw
	sudo install -g 0 -o 0 -m 0644 doc/csw.8 /usr/share/man/man8/
	sudo gzip /usr/share/man/man8/csw.8
remove:
	sudo rm /bin/csw
