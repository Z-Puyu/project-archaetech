extends BST

func _init():
	root = null;
	

func rotateLeft(T: BSTVertex):
	var w: BSTVertex = T.right;
	w.parent = T.parent;
	T.parent = w;
	T.right = w.left;
	if (w.left != null):
		w.left.parent = T;
	w.left = T;
	T.height = getHeight(T);
	w.height = getHeight(w);
	
	return w;
	
func rotateRight(T: BSTVertex):
	var w: BSTVertex = T.left;
	w.parent = T.parent;
	T.parent = w;
	T.left = w.right;
	if (w.right != null):
		w.right.parent = T;
	w.right = T;
	T.height = getHeight(T);
	w.height = getHeight(w);
	
	return w;
	
func AVLinsert(T: BSTVertex, v: Variant):
	insert(T, v);
	
	var balance: int = getHeight(T.left.left) - getHeight(T.right.right);
	if (balance == 2):
		var balance2: int = getHeight(T.left.left) - getHeight(T.left.right);
		if(balance2 == 1):
			T = rotateRight(T);
		else:
			T.left = rotateLeft(T);
			T = rotateRight(T);
	elif(balance == -2):
		var balance2 = getHeight(T.right.left) - getHeight(T.right.right)
		if(balance2 == -1):
			T = rotateLeft(T);
		else:
			T.right = rotateRight(T.right)
			T = rotateLeft(T);
	T.height = getHeight(T);
	return T;
	
func AVLdelete(T:BSTVertex, v: Variant):
	delete(T, v);
	if T != null:
		var balance: int = getHeight(T.left.left) - getHeight(T.right.right);
		if (balance == 2):
			var balance2: int = getHeight(T.left.left) - getHeight(T.left.right);
			if(balance2 == 1):
				T = rotateRight(T);
			else:
				T.left = rotateLeft(T);
				T = rotateRight(T);
		elif(balance == -2):
			var balance2 = getHeight(T.right.left) - getHeight(T.right.right)
			if(balance2 == -1):
				T = rotateLeft(T);
			else:
				T.right = rotateRight(T.right)
				T = rotateLeft(T);
		T.height = getHeight(T);
		return T;
		
	
