using System;
using System.Collections.Generic;
using System.IO;
public class Node
{
# region(Fields)
	public static int Root = 0;
	public static int Folder = 1;
	public static int Item = 2;

	public int Type; // Root=0, Folder=1, Item=2
	public string Name;
	public string Path;
	public string ParentPath;

	public List<Node> List = new List<Node>();
#endregion
    #region(Constructor)
    public Node(string path, int type)
	{
		this.Type = type;
		this.Path = path;
		this.ParentPath = this.Path.Substring(0, this.Path.LastIndexOf("\\"));
		this.Name = this.Path.Substring(this.Path.LastIndexOf("\\") + 1);
	}

	public Node()
    {
		//Default constructor
    }
    #endregion

    #region(Methods)
    public bool IsRoot()
	{
		return this.Type == Root;
	}

	public bool IsFolder()
	{
		return this.Type == Folder;
	}

	public bool IsItem() {
		return this.Type == Item;
	}

	public string GetTypeDisplay() {
		if (this.Type == Root) {
			return "Root:";
		}

		if (this.Type == Folder) {
			return "Folder:";
		}

		if (this.Type == Item) {
			return "Item:";
		}

		return "Undefined";
	}

	public bool IsEmpty() {
		return (this.List.Count == 0);
	}

	public Node Add(string name, int type) {

		if (type == Root) {
			throw new Exception("error: not allowed to add multiple root nodes");
		}

		if (this.Type == Item) {
			throw new Exception("error: unable to add new paths to an item type");
		}

		Node newNode = new Node(this.Path + "\\" + name, type);

		if (newNode.Type == Folder) {
			Directory.CreateDirectory(newNode.Path);
		}

		if (newNode.Type == Item) {
			File.Create(newNode.Path).Close();
		}

		this.List.Add(newNode);
		return newNode;
	}

	public bool Delete()
	{
		if (this.IsRoot())
		{
			throw new Exception("error: not allowed to delete root node");
		}

		if (this.IsItem()) {
			File.Delete(this.Path);
			this.List.Clear();
			return true;
		}

		if (this.IsFolder()) {
			Directory.Delete(this.Path, true);
			this.List.Clear();
			return true;
		}

		throw new Exception("error: unexpected deletion request");
	}

	public bool Move(string destPath)
	{
		this.ParentPath = destPath;
		this.Path = this.ParentPath + "\\" + this.Name;

		foreach (Node node in this.List)
        {
			node.Move(this.Path);
        }

        return true;
	}
    #endregion
}
